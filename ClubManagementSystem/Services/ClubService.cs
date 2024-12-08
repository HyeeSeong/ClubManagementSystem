using ClubManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace ClubManagementSystem.Services;

public class ClubService
{
    private ClubManagementContext context;

    public ClubService(ClubManagementContext context)
    {
        this.context = context;
    }

    public bool InsertClub()
    {
        try
        {
            Console.WriteLine("[동아리 정보 입력]");
            string clubName = GetInput<string>("동아리 이름", input => !string.IsNullOrWhiteSpace(input), "동아리 이름은 빈 값일 수 없습니다.");
            
            // Professor, Staff 지정
            // 기존 Professor, Staff 목록에서 선택하는 로직 가정(없다면 생략 가능)
            var professors = context.Professors.Include(p => p.User).ToList();
            if (!professors.Any())
            {
                Console.WriteLine("등록된 교수가 없습니다. 교수가 있어야 동아리 생성 가능합니다.");
                return false;
            }

            Console.WriteLine("[등록된 교수 목록]");
            foreach (var p in professors)
            {
                Console.WriteLine($"Professor(UserID): {p.UserID}, 이름: {p.User.FirstName} {p.User.LastName}");
            }

            int professorID = GetInput<int>("교수의 UserID를 선택하세요", input => professors.Any(p => p.UserID == int.Parse(input)), "유효한 Professor UserID가 아닙니다.");

            var staffs = context.Staffs.Include(s => s.User).ToList();
            if (!staffs.Any())
            {
                Console.WriteLine("등록된 교직원이 없습니다. 교직원이 있어야 동아리 생성 가능합니다.");
                return false;
            }

            Console.WriteLine("[등록된 교직원 목록]");
            foreach (var s in staffs)
            {
                Console.WriteLine($"Staff(UserID): {s.UserID}, 이름: {s.User.FirstName} {s.User.LastName}");
            }

            int staffID = GetInput<int>("교직원의 UserID를 선택하세요", input => staffs.Any(s => s.UserID == int.Parse(input)), "유효한 Staff UserID가 아닙니다.");

            // ClubRoom 선택 (없어도 가능하므로 Enter로 skip)
            var clubRooms = context.ClubRooms.ToList();
            int? chosenClubRoomID = null;
            if (clubRooms.Any())
            {
                Console.WriteLine("[등록된 동아리방 목록]");
                foreach (var r in clubRooms)
                {
                    Console.WriteLine($"ClubRoomID: {r.ClubRoomID}, Location: {r.Location}, Size: {r.Size}, Status: {r.Status}");
                }
                Console.WriteLine("동아리방의 ClubRoomID를 입력하세요. 없으면 Enter:");
                string roomInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(roomInput) && int.TryParse(roomInput, out int roomID))
                {
                    if (clubRooms.Any(cr => cr.ClubRoomID == roomID))
                    {
                        chosenClubRoomID = roomID;
                    }
                    else
                    {
                        Console.WriteLine("유효하지 않은 ClubRoomID. 동아리방 없이 진행합니다.");
                    }
                }
            }

            var newClub = new Club
            {
                ClubName = clubName,
                ProfessorID = professorID,
                StaffID = staffID,
                ClubRoomID = chosenClubRoomID
            };

            context.Clubs.Add(newClub);
            context.SaveChanges();
            Console.WriteLine("동아리 등록 완료");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
            return false;
        }
    }

    public bool UpdateClub()
    {
        try
        {
            var club = FindClub();
            if (club == null) return true;

            Console.WriteLine($"선택한 동아리 업데이트: {club}");
            Console.WriteLine("[1. 동아리 이름, 2. Professor 변경, 3. Staff 변경, 4. ClubRoom 변경, Enter 종료]");
            while (true)
            {
                Console.Write("속성 번호 입력(Enter 종료): ");
                string option = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(option))
                {
                    Console.WriteLine("업데이트 종료.");
                    break;
                }

                switch (option)
                {
                    case "1":
                        club.ClubName = GetInput<string>("새 동아리 이름(Enter 건너뛰기)", _ => true, "", input => string.IsNullOrWhiteSpace(input) ? club.ClubName : input);
                        Console.WriteLine("동아리 이름 변경 완료.");
                        break;
                    case "2":
                        // 교수 변경 로직 (Professor 목록 표시 후 선택)
                        var professors = context.Professors.Include(p => p.User).ToList();
                        if (!professors.Any())
                        {
                            Console.WriteLine("등록된 교수가 없습니다.");
                            break;
                        }
                        foreach (var p in professors)
                        {
                            Console.WriteLine($"Professor(UserID): {p.UserID}, 이름: {p.User.FirstName} {p.User.LastName}");
                        }
                        int newProfID = GetInput<int>("새로운 교수 UserID (Enter 건너뛰기)", _ => true, "", input =>
                            string.IsNullOrWhiteSpace(input) ? club.ProfessorID : int.Parse(input));
                        // 유효성 체크
                        if (professors.Any(p => p.UserID == newProfID))
                        {
                            club.ProfessorID = newProfID;
                            Console.WriteLine("교수 변경 완료.");
                        }
                        else
                        {
                            Console.WriteLine("유효하지 않은 교수 ID. 변경 취소.");
                        }
                        break;
                    case "3":
                        var staffs = context.Staffs.Include(s => s.User).ToList();
                        if (!staffs.Any())
                        {
                            Console.WriteLine("등록된 교직원이 없습니다.");
                            break;
                        }
                        foreach (var s in staffs)
                        {
                            Console.WriteLine($"Staff(UserID): {s.UserID}, 이름: {s.User.FirstName} {s.User.LastName}");
                        }
                        int newStaffID = GetInput<int>("새로운 교직원 UserID (Enter 건너뛰기)", _ => true, "", input =>
                            string.IsNullOrWhiteSpace(input) ? club.StaffID : int.Parse(input));
                        if (staffs.Any(st => st.UserID == newStaffID))
                        {
                            club.StaffID = newStaffID;
                            Console.WriteLine("교직원 변경 완료.");
                        }
                        else
                        {
                            Console.WriteLine("유효하지 않은 교직원 ID. 변경 취소.");
                        }
                        break;
                    case "4":
                        var clubRooms = context.ClubRooms.ToList();
                        if (clubRooms.Any())
                        {
                            foreach (var r in clubRooms)
                            {
                                Console.WriteLine($"ClubRoomID: {r.ClubRoomID}, {r.Location}");
                            }
                            int? newRoomID = GetInput<int?>("새로운 ClubRoomID(Enter 건너뛰기)", _ => true, "", input =>
                                string.IsNullOrWhiteSpace(input) ? club.ClubRoomID : int.Parse(input));
                            if (newRoomID.HasValue && clubRooms.Any(cr => cr.ClubRoomID == newRoomID))
                            {
                                club.ClubRoomID = newRoomID;
                                Console.WriteLine("ClubRoom 변경 완료.");
                            }
                            else
                            {
                                Console.WriteLine("유효하지 않은 ClubRoomID. 변경 취소.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("등록된 ClubRoom이 없습니다.");
                        }
                        break;
                    default:
                        Console.WriteLine("유효하지 않은 선택입니다.");
                        break;
                }
            }

            context.SaveChanges();
            Console.WriteLine("동아리 정보 업데이트 완료.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
            return false;
        }
    }

    public bool DeleteClub()
    {
        try
        {
            var club = FindClub();
            if (club == null) return true;

            Console.WriteLine($"선택한 동아리 삭제: {club}");
            Console.WriteLine("정말 삭제하시겠습니까?(Y/N)");
            string ans = Console.ReadLine()?.Trim().ToUpper();
            if (ans == "Y")
            {
                context.Clubs.Remove(club);
                context.SaveChanges();
                Console.WriteLine("동아리 삭제 완료.");
            }
            else
            {
                Console.WriteLine("삭제 취소.");
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
            return false;
        }
    }

    public void PrintAllClubs()
    {
        try
        {
            var clubs = context.Clubs.Include(c => c.Professor.User).Include(c => c.Staff.User).ToList();
            if (!clubs.Any())
            {
                Console.WriteLine("등록된 동아리가 없습니다.");
                return;
            }

            Console.WriteLine("[동아리 목록]");
            foreach (var c in clubs)
            {
                Console.WriteLine($"[{c.ClubID}] ClubName: {c.ClubName}, Professor: {c.Professor.User.FirstName} {c.Professor.User.LastName}, Staff: {c.Staff.User.FirstName} {c.Staff.User.LastName}, ClubRoomID: {(c.ClubRoomID.HasValue ? c.ClubRoomID.ToString() : "(없음)")}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
        }
    }

    private Club? FindClub()
    {
        while (true)
        {
            Console.Write("동아리를 검색합니다. 동아리 이름 또는 ClubID를 입력하세요(Enter 취소): ");
            string input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input)) return null;

            if (int.TryParse(input, out int cid))
            {
                var clubByID = context.Clubs.FirstOrDefault(cl => cl.ClubID == cid);
                if (clubByID != null) return clubByID;
                Console.WriteLine("해당 ClubID가 없습니다.");
                continue;
            }

            var clubsByName = context.Clubs.Where(cl => cl.ClubName.Contains(input)).ToList();
            if (!clubsByName.Any())
            {
                Console.WriteLine("검색 결과가 없습니다.");
                continue;
            }

            if (clubsByName.Count == 1)
            {
                return clubsByName.First();
            }

            Console.WriteLine("[검색 결과]");
            foreach (var c in clubsByName)
            {
                Console.WriteLine($"ClubID: {c.ClubID}, ClubName: {c.ClubName}");
            }

            Console.WriteLine("위 목록에서 ClubID를 입력하세요(Enter 취소):");
            string idInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(idInput)) return null;

            if (int.TryParse(idInput, out int chosenID))
            {
                var selected = clubsByName.FirstOrDefault(cl => cl.ClubID == chosenID);
                if (selected != null) return selected;
            }
            Console.WriteLine("유효하지 않은 ClubID입니다.");
        }
    }

    // 공통 입력 함수 재사용
    static T GetInput<T>(string prompt, Func<string, bool> validator, string errorMessage = "유효하지 않은 입력입니다.", Func<string, T> parser = null)
    {
        while (true)
        {
            Console.Write($"{prompt}: ");
            string input = Console.ReadLine();

            if (validator(input))
            {
                try
                {
                    return parser != null ? parser(input) : (T)Convert.ChangeType(input, typeof(T));
                }
                catch
                {
                    Console.WriteLine("입력값 처리 중 오류 발생.");
                }
            }

            Console.WriteLine(errorMessage);
        }
    }
}

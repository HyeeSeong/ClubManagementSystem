using ClubManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace ClubManagementSystem.Services;

public class ClubRoomService
{
    private readonly ClubManagementContext context;

    public ClubRoomService(ClubManagementContext context)
    {
        this.context = context;
    }

    // ClubRoom 추가
    public bool InsertClubRoom()
    {
        try
        {
            Console.WriteLine("[동아리방 정보 입력]");
            string location = GetInput<string>("위치", input => !string.IsNullOrWhiteSpace(input), "위치는 빈 값일 수 없습니다.");
            int size = GetInput<int>("크기(평수)", input => int.TryParse(input, out int result) && result > 0, "유효한 숫자를 입력하세요.");
            string status = GetInput<string>("상태(예: 사용 가능, 유지보수 중 등)", input => !string.IsNullOrWhiteSpace(input), "상태는 빈 값일 수 없습니다.");

            var newClubRoom = new ClubRoom
            {
                Location = location,
                Size = size,
                Status = status
            };

            context.ClubRooms.Add(newClubRoom);
            context.SaveChanges();
            Console.WriteLine("동아리방이 성공적으로 추가되었습니다.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
            return false;
        }
    }

    // ClubRoom 업데이트
    public bool UpdateClubRoom()
    {
        try
        {
            var clubRoom = FindClubRoom();
            if (clubRoom == null) return true;

            Console.WriteLine($"선택한 동아리방: ClubRoomID = {clubRoom.ClubRoomID}, Location = {clubRoom.Location}");
            Console.WriteLine("[1. 위치 변경, 2. 크기 변경, 3. 상태 변경, Enter 종료]");
            while (true)
            {
                Console.Write("옵션을 선택하세요(Enter 종료): ");
                string option = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(option)) break;

                switch (option)
                {
                    case "1":
                        clubRoom.Location = GetInput<string>("새로운 위치(Enter 건너뛰기)", _ => true, "", input => string.IsNullOrWhiteSpace(input) ? clubRoom.Location : input);
                        Console.WriteLine("위치가 변경되었습니다.");
                        break;
                    case "2":
                        clubRoom.Size = GetInput<int>("새로운 크기(Enter 건너뛰기)", input => string.IsNullOrWhiteSpace(input) || int.TryParse(input, out int result) && result > 0, "유효하지 않은 크기입니다.", input =>
                            string.IsNullOrWhiteSpace(input) ? clubRoom.Size : int.Parse(input));
                        Console.WriteLine("크기가 변경되었습니다.");
                        break;
                    case "3":
                        clubRoom.Status = GetInput<string>("새로운 상태(Enter 건너뛰기)", _ => true, "", input => string.IsNullOrWhiteSpace(input) ? clubRoom.Status : input);
                        Console.WriteLine("상태가 변경되었습니다.");
                        break;
                    default:
                        Console.WriteLine("유효하지 않은 선택입니다.");
                        break;
                }
            }

            context.SaveChanges();
            Console.WriteLine("동아리방 정보가 성공적으로 업데이트되었습니다.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
            return false;
        }
    }

    // ClubRoom 삭제
    public bool DeleteClubRoom()
    {
        try
        {
            var clubRoom = FindClubRoom();
            if (clubRoom == null) return true;

            Console.WriteLine($"선택한 동아리방: ClubRoomID = {clubRoom.ClubRoomID}, Location = {clubRoom.Location}");
            Console.WriteLine("정말 삭제하시겠습니까? (Y/N)");
            string confirmation = Console.ReadLine()?.Trim().ToUpper();
            if (confirmation == "Y")
            {
                context.ClubRooms.Remove(clubRoom);
                context.SaveChanges();
                Console.WriteLine("동아리방이 성공적으로 삭제되었습니다.");
                return true;
            }

            Console.WriteLine("삭제가 취소되었습니다.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
            return false;
        }
    }

    // 모든 ClubRoom 출력
    public void PrintAllClubRooms()
    {
        try
        {
            var clubRooms = context.ClubRooms.ToList();
            if (!clubRooms.Any())
            {
                Console.WriteLine("등록된 동아리방이 없습니다.");
                return;
            }

            Console.WriteLine("[동아리방 목록]");
            foreach (var room in clubRooms)
            {
                Console.WriteLine($"ClubRoomID: {room.ClubRoomID}, Location: {room.Location}, Size: {room.Size}, Status: {room.Status}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
        }
    }

    // ClubRoom 찾기
    private ClubRoom? FindClubRoom()
    {
        while (true)
        {
            Console.WriteLine("동아리방을 검색합니다. Location 또는 ClubRoomID를 입력하세요 (빈 값 입력 시 검색 취소):");
            string input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input)) return null;

            if (int.TryParse(input, out int id))
            {
                var clubRoomByID = context.ClubRooms.FirstOrDefault(r => r.ClubRoomID == id);
                if (clubRoomByID != null) return clubRoomByID;
                Console.WriteLine("해당 ClubRoomID가 없습니다.");
                continue;
            }

            var clubRoomsByLocation = context.ClubRooms.Where(r => r.Location.Contains(input)).ToList();
            if (!clubRoomsByLocation.Any())
            {
                Console.WriteLine("검색 결과가 없습니다.");
                continue;
            }

            if (clubRoomsByLocation.Count == 1) return clubRoomsByLocation.First();

            Console.WriteLine("[검색 결과]");
            foreach (var room in clubRoomsByLocation)
            {
                Console.WriteLine($"ClubRoomID: {room.ClubRoomID}, Location: {room.Location}, Size: {room.Size}, Status: {room.Status}");
            }

            Console.WriteLine("위 목록에서 ClubRoomID를 선택하세요 (빈 값 입력 시 취소):");
            string idInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(idInput)) return null;

            if (int.TryParse(idInput, out int selectedID))
            {
                var selectedRoom = clubRoomsByLocation.FirstOrDefault(r => r.ClubRoomID == selectedID);
                if (selectedRoom != null) return selectedRoom;
            }

            Console.WriteLine("유효하지 않은 ClubRoomID입니다.");
        }
    }

    // 공통 입력 함수
    private static T GetInput<T>(string prompt, Func<string, bool> validator, string errorMessage = "유효하지 않은 입력입니다.", Func<string, T> parser = null)
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

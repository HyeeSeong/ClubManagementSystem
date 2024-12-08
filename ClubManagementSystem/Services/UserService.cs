using ClubManagementSystem.Domain;

namespace ClubManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

public class UserService
{
    private ClubManagementContext context;

    public UserService(ClubManagementContext context)
    {
        this.context = context;
    }
    
    public bool InsertUser()
    {
        try
        {
            // 유저 정보 입력
            Console.WriteLine("[유저 정보 입력]");
            string FirstName = GetInput<string>("성", input => !string.IsNullOrWhiteSpace(input), "성은 빈 값일 수 없습니다.");
            string LastName = GetInput<string>("이름", input => !string.IsNullOrWhiteSpace(input), "이름은 빈 값일 수 없습니다.");
            string Email;
            while (true)
            {
                Email = GetInput<string>("이메일", input => !string.IsNullOrWhiteSpace(input), "이메일은 빈 값일 수 없습니다.");
                if (!context.Users.Any(u => u.Email == Email))
                {
                    break; // 이메일이 중복되지 않으면 루프 종료
                }
                Console.WriteLine("이미 존재하는 이메일입니다. 다른 이메일을 입력해주세요.");
            }
            string Phone = GetInput<string>("휴대폰 번호", input => !string.IsNullOrWhiteSpace(input), "휴대폰 번호는 빈 값일 수 없습니다.");
            DateTime Birthday = GetInput<DateTime>(
                "생년월일을 입력해주세요 (예: 1990-12-25)", 
                input => DateTime.TryParse(input, out _), 
                "유효한 날짜 형식으로 입력해주세요.",
                input => DateTime.Parse(input)
            );
            string Description = GetInput<string>("추가 정보", _ => true); // 빈 값 허용
            
            // 입력 정보 확인
            Console.WriteLine("\n=========");
            Console.WriteLine("[입력한 정보]");
            Console.WriteLine($"성: {FirstName}");
            Console.WriteLine($"이름: {LastName}");
            Console.WriteLine($"이메일: {Email}");
            Console.WriteLine($"휴대폰 번호: {Phone}");
            Console.WriteLine($"생년월일: {Birthday:yyyy-MM-dd}");
            Console.WriteLine("추가 정보: " + (string.IsNullOrWhiteSpace(Description) ? "(입력 없음)" : Description));
            Console.WriteLine("=========\n");
            
            // 객체 생성
            var newUser = new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                PhoneNumber = Phone,
                Birth = Birthday,
                Description = Description,
            };

            // 유저 Insert
            context.Users.Add(newUser);
            context.SaveChanges();

            // 유저 타입 입력
            Console.WriteLine("유저 타입을 선택해주세요 (1. 학생, 2. 교수, 3. 교직원)");
            string type = Console.ReadLine();

            while (true)
            {
                switch (type)
                {
                    case "1": // 학생인 경우
                    {
                        string status = GetInput<string>("상태(재학, 휴학)", input => !string.IsNullOrWhiteSpace(input), "상태는 빈 값일 수 없습니다.");
                        int year = GetInput<int>("학년", input => !string.IsNullOrWhiteSpace(input), "학년은 빈 값일 수 없습니다.");

                        // 동아리 선택 여부
                        var clubs = context.Clubs.ToList();
                        int? chosenClubID = null;

                        if (clubs.Count > 0)
                        {
                            Console.WriteLine("[현재 개설된 동아리 목록]");
                            foreach (var c in clubs)
                            {
                                Console.WriteLine(c.ToString());
                            }

                            // 클럽 선택
                            Console.WriteLine("해당하는 동아리의 ClubID를 입력하세요. 동아리에 속하지 않으려면 빈 값으로 Enter를 누르세요.");
                            string clubInput = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(clubInput) && int.TryParse(clubInput, out int clubID))
                            {
                                if (context.Clubs.Any(u => u.ClubID == clubID))
                                {
                                    chosenClubID = clubID;
                                }
                                else
                                {
                                    Console.WriteLine("유효하지 않은 ClubID입니다. 동아리 없음으로 처리합니다.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("현재 개설된 동아리가 없습니다. 동아리 정보 없이 학생을 등록합니다.");
                        }

                        var newStudent = new Student
                        {
                            UserID = newUser.UserID,  // 위에서 Insert한 유저의 UserID 사용
                            Status = status,
                            Year = year,
                            ClubID = chosenClubID
                        };

                        context.Students.Add(newStudent);
                        context.SaveChanges();
                        Console.WriteLine("학생 정보가 성공적으로 저장되었습니다.");
                        return true;
                    }
                    case "2": // 교수인 경우
                    {
                        string department = GetInput<string>("학과", input => !string.IsNullOrWhiteSpace(input), "학과는 빈 값일 수 없습니다.");
                        var newProfessor = new Professor
                        {
                            UserID = newUser.UserID,
                        };
                        
                        context.Professors.Add(newProfessor);
                        context.SaveChanges();
                        Console.WriteLine("교수 정보가 성공적으로 저장되었습니다.");
                        return true;
                    }
                    case "3": // 교직원인 경우
                    {
                        string role = GetInput<string>("역할", input => !string.IsNullOrWhiteSpace(input), "역할은 빈 값일 수 없습니다.");
                        var newStaff = new Staff
                        {
                            UserID = newUser.UserID,
                        };

                        context.Staffs.Add(newStaff);
                        context.SaveChanges();
                        Console.WriteLine("교직원 정보가 성공적으로 저장되었습니다.");
                        return true;
                    }
                    default:
                    {
                        Console.WriteLine("유효하지 않은 값입니다. 다시 입력해주세요 (1. 학생, 2. 교수, 3. 교직원)");
                        type = Console.ReadLine();
                        break;
                    }
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("오류 발생 User Insert 실패");
            return false;
        }
    }
    
    // 제네릭 입력 함수
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
                    Console.WriteLine("입력값을 처리하는 중 오류가 발생했습니다.");
                }
            }

            Console.WriteLine(errorMessage);
        }
    }
}
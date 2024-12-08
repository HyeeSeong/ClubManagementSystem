using ClubManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace ClubManagementSystem.Services;

public class NotificationService
{
    private ClubManagementContext context;

    public NotificationService(ClubManagementContext context)
    {
        this.context = context;
    }

    public bool InsertNotification()
    {
        try
        {
            Console.WriteLine("[공지사항 등록]");
            string desc = GetInput<string>("공지 내용", input => !string.IsNullOrWhiteSpace(input), "공지 내용은 빈 값일 수 없습니다.");
            DateTime date = DateTime.Today; // 기본 현재 날짜 사용

            // Notification 생성 및 저장
            var notification = new Notification
            {
                Description = desc,
                Date = date
            };

            context.Notifications.Add(notification);
            context.SaveChanges(); // Notification 저장하여 AnnounceID 확보
            Console.WriteLine("공지 등록 완료");

            // 공지 대상 추가
            Console.WriteLine("[알림 대상을 추가합니다]");
            AddNotifiesFromInput(notification.AnnounceID);

            context.SaveChanges();
            Console.WriteLine("공지 및 알림 대상 등록 완료");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
            return false;
        }
    }

    public bool UpdateNotification()
    {
        try
        {
            var notification = FindNotification();
            if (notification == null) return true;

            Console.WriteLine($"선택한 공지 업데이트: AnnounceID = {notification.AnnounceID}, Desc = {notification.Description}");
            Console.WriteLine("[1. 내용 변경, 2. 날짜 변경, 3. 공지 대상 변경, Enter 종료]");
            while (true)
            {
                Console.Write("속성 번호(Enter 종료): ");
                string option = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(option)) break;

                switch (option)
                {
                    case "1":
                        notification.Description = GetInput<string>("새 내용(Enter 건너뛰기)", _ => true, "", input => string.IsNullOrWhiteSpace(input) ? notification.Description : input);
                        Console.WriteLine("내용 변경 완료.");
                        break;

                    case "2":
                        notification.Date = GetInput<DateTime>("새 날짜(YYYY-MM-DD)", input => DateTime.TryParse(input, out _), "유효하지 않은 날짜", input => DateTime.Parse(input));
                        Console.WriteLine("날짜 변경 완료.");
                        break;

                    case "3": // 공지 대상 변경
                        UpdateNotifies(notification.AnnounceID);
                        break;

                    default:
                        Console.WriteLine("유효하지 않은 선택.");
                        break;
                }
            }

            context.SaveChanges();
            Console.WriteLine("공지 업데이트 완료.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
            return false;
        }
    }
    
    public bool DeleteNotification()
    {
        try
        {
            var notification = FindNotification();
            if (notification == null) return true;

            Console.WriteLine($"공지 삭제: {notification.AnnounceID}, {notification.Description}");
            Console.WriteLine("정말 삭제하시겠습니까?(Y/N)");
            string ans = Console.ReadLine()?.Trim().ToUpper();
            if (ans == "Y")
            {
                context.Notifications.Remove(notification);
                context.SaveChanges();
                Console.WriteLine("공지 삭제 완료.");
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

    // 공지 대상 변경 로직
    private void UpdateNotifies(int notificationID)
    {
        Console.WriteLine("[공지 대상 변경]");
        var existingNotifies = context.Notifies
            .Where(n => n.NotificationID == notificationID)
            .Select(n => n.UserID)
            .ToList();

        // 기존 공지 대상 출력
        if (existingNotifies.Any())
        {
            Console.WriteLine("[현재 공지 대상]");
            foreach (var userId in existingNotifies)
            {
                var user = context.Users.Find(userId);
                if (user != null)
                {
                    Console.WriteLine($"UserID: {user.UserID}, Name: {user.FirstName} {user.LastName}, Email: {user.Email}");
                }
            }
        }
        else
        {
            Console.WriteLine("현재 공지 대상이 없습니다.");
        }

        Console.WriteLine("[1. 공지 대상 추가, 2. 공지 대상 제거, Enter 종료]");
        while (true)
        {
            Console.Write("옵션을 선택하세요(Enter 종료): ");
            string option = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(option)) break;

            switch (option)
            {
                case "1": // 공지 대상 추가
                    AddNotifiesFromInput(notificationID);
                    break;

                case "2": // 공지 대상 제거
                    RemoveNotifiesFromInput(notificationID, existingNotifies);
                    break;

                default:
                    Console.WriteLine("유효하지 않은 선택입니다.");
                    break;
            }
        }
    }

        
    // Notifies 추가 메서드
    private void AddNotifies(int notificationID, List<int> userIds)
    {
        // 기존에 추가된 UserID 확인
        var existingUserIds = context.Notifies
            .Where(n => n.NotificationID == notificationID)
            .Select(n => n.UserID)
            .ToHashSet();

        // 새로운 UserID 중 중복되지 않은 것만 추가
        foreach (var userId in userIds)
        {
            if (!existingUserIds.Contains(userId))
            {
                context.Notifies.Add(new Notifies
                {
                    NotificationID = notificationID,
                    UserID = userId
                });
            }
        }
    }
    
    // 알림 대상 추가
    private void AddNotifiesFromInput(int notificationID)
    {
        Console.WriteLine("[알림 대상을 추가합니다]");
        Console.WriteLine("[" + 
                          "1. 전체 유저, " +
                          "2. 교직원, " +
                          "3. 교수, " +
                          "4. 특정 동아리 소속 학생, " +
                          "5. 특정 프로젝트 소속 학생, " +
                          "6. 전체 학생, " +
                          "Enter를 눌러 종료" +
                          "]");
        while (true)
        {
            Console.Write("옵션을 선택하세요 (Enter 종료): ");
            string input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input)) break; // 종료

            switch (input)
            {
                case "1": // 전체 유저
                    var allUsers = context.Users.Select(u => u.UserID).ToList();
                    AddNotifies(notificationID, allUsers);
                    Console.WriteLine("전체 유저에게 알림이 설정되었습니다.");
                    break;

                case "2": // 교직원
                    var staffUsers = context.Staffs.Select(s => s.UserID).ToList();
                    AddNotifies(notificationID, staffUsers);
                    Console.WriteLine("교직원에게 알림이 설정되었습니다.");
                    break;

                case "3": // 교수
                    var professorUsers = context.Professors.Select(p => p.UserID).ToList();
                    AddNotifies(notificationID, professorUsers);
                    Console.WriteLine("교수에게 알림이 설정되었습니다.");
                    break;

                case "4": // 특정 동아리 소속 학생
                    var clubs = context.Clubs.ToList();
                    if (!clubs.Any())
                    {
                        Console.WriteLine("등록된 동아리가 없습니다.");
                        break;
                    }
                    Console.WriteLine("[등록된 동아리 목록]");
                    foreach (var club in clubs)
                    {
                        Console.WriteLine($"ClubID: {club.ClubID}, ClubName: {club.ClubName}");
                    }
                    int clubID = GetInput<int>("알림을 보낼 동아리 ClubID", input => clubs.Any(c => c.ClubID == int.Parse(input)), "유효하지 않은 ClubID입니다.");
                    var clubStudents = context.Students.Where(s => s.ClubID == clubID).Select(s => s.UserID).ToList();
                    AddNotifies(notificationID, clubStudents);
                    Console.WriteLine($"ClubID {clubID} 소속 학생들에게 알림이 설정되었습니다.");
                    break;

                case "5": // 특정 프로젝트 소속 학생
                    var projects = context.Projects.ToList();
                    if (!projects.Any())
                    {
                        Console.WriteLine("등록된 프로젝트가 없습니다.");
                        break;
                    }
                    Console.WriteLine("[등록된 프로젝트 목록]");
                    foreach (var project in projects)
                    {
                        Console.WriteLine($"ProjectID: {project.ProjectID}, ProjectName: {project.Name}");
                    }
                    int projectID = GetInput<int>("알림을 보낼 프로젝트 ProjectID", input => projects.Any(p => p.ProjectID == int.Parse(input)), "유효하지 않은 ProjectID입니다.");
                    var projectStudents = context.Participations
                        .Where(p => p.ProjectID == projectID)
                        .Select(p => p.StudentID)
                        .ToList();
                    AddNotifies(notificationID, projectStudents);
                    Console.WriteLine($"ProjectID {projectID} 소속 학생들에게 알림이 설정되었습니다.");
                    break;

                case "6": // 전체 학생
                    var allStudents = context.Students.Select(s => s.UserID).ToList();
                    AddNotifies(notificationID, allStudents);
                    Console.WriteLine("전체 학생에게 알림이 설정되었습니다.");
                    break;

                default:
                    Console.WriteLine("유효하지 않은 옵션입니다. 다시 선택해주세요.");
                    break;
            }
        }
    }

    //알림 대상 제거
    private void RemoveNotifiesFromInput(int notificationID, List<int> existingUserIds)
    {
        Console.WriteLine("[알림 대상을 제거합니다]");
        foreach (var userId in existingUserIds)
        {
            var user = context.Users.Find(userId);
            if (user != null)
            {
                Console.WriteLine($"UserID: {user.UserID}, Name: {user.FirstName} {user.LastName}, Email: {user.Email}");
            }
        }

        while (true)
        {
            Console.Write("제거할 UserID를 입력하세요 (Enter 종료): ");
            string input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input)) break;

            if (int.TryParse(input, out int userId))
            {
                var notify = context.Notifies.FirstOrDefault(n => n.NotificationID == notificationID && n.UserID == userId);
                if (notify != null)
                {
                    context.Notifies.Remove(notify);
                    Console.WriteLine($"UserID {userId} 공지 대상에서 제거되었습니다.");
                }
                else
                {
                    Console.WriteLine("해당 UserID는 공지 대상이 아닙니다.");
                }
            }
            else
            {
                Console.WriteLine("유효하지 않은 UserID입니다.");
            }
        }
    }

    

    public void PrintAllNotifications()
    {
        try
        {
            var notifications = context.Notifications.ToList();
            if (!notifications.Any())
            {
                Console.WriteLine("등록된 공지가 없습니다.");
                return;
            }

            Console.WriteLine("[공지 목록]");
            foreach (var n in notifications)
            {
                Console.WriteLine($"AnnounceID: {n.AnnounceID}, Desc: {n.Description}, Date: {n.Date:yyyy-MM-dd}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
        }
    }

    private Notification? FindNotification()
    {
        while (true)
        {
            Console.WriteLine("공지 검색. AnnounceID 또는 내용 일부 입력(Enter 취소):");
            string input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input)) return null;

            if (int.TryParse(input, out int id))
            {
                var byID = context.Notifications.FirstOrDefault(n => n.AnnounceID == id);
                if (byID != null) return byID;
                Console.WriteLine("해당 AnnounceID가 없습니다.");
                continue;
            }

            var byDesc = context.Notifications.Where(n => n.Description.Contains(input)).ToList();
            if (!byDesc.Any())
            {
                Console.WriteLine("검색 결과 없음.");
                continue;
            }

            if (byDesc.Count == 1)
            {
                return byDesc.First();
            }

            Console.WriteLine("[검색 결과]");
            foreach (var n in byDesc)
            {
                Console.WriteLine($"AnnounceID: {n.AnnounceID}, Desc: {n.Description}, Date: {n.Date:yyyy-MM-dd}");
            }

            Console.WriteLine("위 목록에서 AnnounceID를 입력하세요(Enter 취소):");
            string idInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(idInput)) return null;

            if (int.TryParse(idInput, out int chosenID))
            {
                var selected = byDesc.FirstOrDefault(n => n.AnnounceID == chosenID);
                if (selected != null) return selected;
            }

            Console.WriteLine("유효하지 않은 AnnounceID.");
        }
    }

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

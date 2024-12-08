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
            DateTime date = DateTime.Today; // 기본 현재 날짜
            // 필요하다면 date 입력 로직 추가 가능

            var notification = new Notification
            {
                Description = desc,
                Date = date
            };

            context.Notifications.Add(notification);
            context.SaveChanges();
            Console.WriteLine("공지 등록 완료");
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
            Console.WriteLine("[1. 내용 변경, 2. 날짜 변경, Enter 종료]");
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

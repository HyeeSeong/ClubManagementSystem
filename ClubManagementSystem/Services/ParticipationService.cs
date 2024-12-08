using ClubManagementSystem.Domain;

namespace ClubManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

public class ParticipationService
{
    private ClubManagementContext context;

    public ParticipationService(ClubManagementContext context)
    {
        this.context = context;
    }

    public bool InsertParticipation()
    {
        try
        {
            // Student 선택
            var studentID = GetUserIDByRole("Student");
            if (studentID == null) return true;

            // Project 선택
            var projectID = GetProjectID();
            if (projectID == null) return true;

            // 중복 확인
            if (context.Participations.Any(p => p.StudentID == studentID && p.ProjectID == projectID))
            {
                Console.WriteLine("이미 참여 관계가 존재합니다.");
                return true;
            }

            var participation = new Participation
            {
                StudentID = studentID.Value,
                ProjectID = projectID.Value
            };

            context.Participations.Add(participation);
            context.SaveChanges();
            Console.WriteLine("참여 정보가 추가되었습니다.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("오류 발생: " + ex.Message);
            return false;
        }
    }

    public bool DeleteParticipation()
    {
        try
        {
            // Participation 찾기
            var part = FindParticipation();
            if (part == null) return true;

            Console.WriteLine($"StudentID: {part.StudentID}, ProjectID: {part.ProjectID} 참여 삭제");
            Console.Write("정말 삭제하시겠습니까? (Y/N): ");
            string ans = Console.ReadLine()?.Trim().ToUpper();
            if (ans == "Y")
            {
                context.Participations.Remove(part);
                context.SaveChanges();
                Console.WriteLine("참여 정보 삭제 완료.");
            }
            else
            {
                Console.WriteLine("삭제 취소.");
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("오류 발생: " + ex.Message);
            return false;
        }
    }

    public void PrintAllParticipations()
    {
        var participations = context.Participations.ToList();
        if (!participations.Any())
        {
            Console.WriteLine("등록된 참여 정보가 없습니다.");
            return;
        }

        Console.WriteLine("[참여 정보 목록]");
        foreach (var p in participations)
        {
            Console.WriteLine($"StudentID: {p.StudentID}, ProjectID: {p.ProjectID}");
        }
    }

    private Participation? FindParticipation()
    {
        // StudentID와 ProjectID를 직접 입력받아 해당 관계를 찾는다.
        var studentID = GetUserIDByRole("Student");
        if (studentID == null) return null;

        var projectID = GetProjectID();
        if (projectID == null) return null;

        var part = context.Participations.FirstOrDefault(p => p.StudentID == studentID && p.ProjectID == projectID);
        if (part == null) Console.WriteLine("해당 참여 정보가 없습니다.");
        return part;
    }

    private int? GetUserIDByRole(string role) // role: "Student"
    {
        if (role == "Student")
        {
            var students = context.Students.Include(s => s.User).ToList();
            if (!students.Any())
            {
                Console.WriteLine("등록된 학생이 없습니다.");
                return null;
            }

            Console.WriteLine("[학생 목록]");
            foreach (var s in students)
            {
                Console.WriteLine($"[{s.UserID}] Name: {s.User.FirstName} {s.User.LastName}");
            }

            int id = GetInput<int>("참여 학생ID", input => students.Any(st => st.UserID == int.Parse(input)), "유효하지 않은 UserID");
            return id;
        }

        return null;
    }

    private int? GetProjectID()
    {
        var projects = context.Projects.ToList();
        if (!projects.Any())
        {
            Console.WriteLine("등록된 프로젝트가 없습니다.");
            return null;
        }

        Console.WriteLine("[프로젝트 목록]");
        foreach (var p in projects)
        {
            Console.WriteLine($"[{p.ProjectID}] Name: {p.Name}");
        }

        int pid = GetInput<int>("ProjectID", input => projects.Any(pr => pr.ProjectID == int.Parse(input)), "유효하지 않은 ProjectID");
        return pid;
    }

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

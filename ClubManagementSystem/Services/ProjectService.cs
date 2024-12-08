using ClubManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace ClubManagementSystem.Services;

public class ProjectService
{
    private ClubManagementContext context;

    public ProjectService(ClubManagementContext context)
    {
        this.context = context;
    }

    public bool InsertProject()
    {
        try
        {
            Console.WriteLine("[프로젝트 정보 입력]");
            string name = GetInput<string>("프로젝트 이름", input => !string.IsNullOrWhiteSpace(input), "프로젝트 이름은 빈 값일 수 없습니다.");

            var clubs = context.Clubs.ToList();
            if (!clubs.Any())
            {
                Console.WriteLine("등록된 동아리가 없어 프로젝트를 생성할 수 없습니다.");
                return false;
            }

            Console.WriteLine("[동아리 목록]");
            foreach (var c in clubs)
            {
                Console.WriteLine($"ClubID: {c.ClubID}, ClubName: {c.ClubName}");
            }

            int clubID = GetInput<int>("프로젝트가 속한 ClubID", input => clubs.Any(cl => cl.ClubID == int.Parse(input)), "유효한 ClubID 아님.");

            var newProject = new Project
            {
                Name = name,
                ClubID = clubID
            };
            context.Projects.Add(newProject);
            context.SaveChanges();
            Console.WriteLine("프로젝트 생성 완료");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
            return false;
        }
    }

    public bool UpdateProject()
    {
        try
        {
            var project = FindProject();
            if (project == null) return true;

            Console.WriteLine($"프로젝트 업데이트: {project.Name}, ProjectID: {project.ProjectID}");
            Console.WriteLine("[1. 이름 변경, 2. 소속 동아리 변경, Enter 종료]");
            while (true)
            {
                Console.Write("속성 번호 입력(Enter 종료): ");
                string option = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(option)) break;

                switch (option)
                {
                    case "1":
                        project.Name = GetInput<string>("새 프로젝트 이름(Enter 건너뛰기)", _ => true, "", input => string.IsNullOrWhiteSpace(input) ? project.Name : input);
                        Console.WriteLine("프로젝트 이름 변경 완료.");
                        break;
                    case "2":
                        var clubs = context.Clubs.ToList();
                        if (!clubs.Any())
                        {
                            Console.WriteLine("동아리가 없습니다.");
                            break;
                        }
                        foreach (var c in clubs)
                        {
                            Console.WriteLine($"ClubID: {c.ClubID}, ClubName: {c.ClubName}");
                        }
                        int newClubID = GetInput<int>("새로운 ClubID(Enter 건너뛰기)", _ => true, "", input =>
                            string.IsNullOrWhiteSpace(input) ? project.ClubID : int.Parse(input));
                        if (clubs.Any(cl => cl.ClubID == newClubID))
                        {
                            project.ClubID = newClubID;
                            Console.WriteLine("소속 동아리 변경 완료.");
                        }
                        else
                        {
                            Console.WriteLine("유효하지 않은 ClubID. 변경 취소.");
                        }
                        break;
                    default:
                        Console.WriteLine("유효하지 않은 선택.");
                        break;
                }
            }

            context.SaveChanges();
            Console.WriteLine("프로젝트 업데이트 완료.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
            return false;
        }
    }

    public bool DeleteProject()
    {
        try
        {
            var project = FindProject();
            if (project == null) return true;

            Console.WriteLine($"프로젝트 삭제: {project.Name}(ID: {project.ProjectID})");
            Console.WriteLine("정말 삭제하시겠습니까?(Y/N)");
            string ans = Console.ReadLine()?.Trim().ToUpper();
            if (ans == "Y")
            {
                context.Projects.Remove(project);
                context.SaveChanges();
                Console.WriteLine("프로젝트 삭제 완료.");
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

    public void PrintAllProjects()
    {
        try
        {
            var projects = context.Projects.Include(p => p.Club).ToList();
            if (!projects.Any())
            {
                Console.WriteLine("등록된 프로젝트가 없습니다.");
                return;
            }

            Console.WriteLine("[프로젝트 목록]");
            foreach (var p in projects)
            {
                Console.WriteLine($"ProjectID: {p.ProjectID}, Name: {p.Name}, ClubID: {p.ClubID}, ClubName: {p.Club.ClubName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
        }
    }

    private Project? FindProject()
    {
        while (true)
        {
            Console.WriteLine("프로젝트 검색. 이름 또는 ProjectID 입력(Enter 취소):");
            string input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input)) return null;

            if (int.TryParse(input, out int pid))
            {
                var byID = context.Projects.FirstOrDefault(pr => pr.ProjectID == pid);
                if (byID != null) return byID;
                Console.WriteLine("해당 ProjectID가 없습니다.");
                continue;
            }

            var byName = context.Projects.Where(pr => pr.Name.Contains(input)).ToList();
            if (!byName.Any())
            {
                Console.WriteLine("검색 결과 없음.");
                continue;
            }
            if (byName.Count == 1) return byName.First();

            Console.WriteLine("[검색 결과]");
            foreach (var p in byName)
            {
                Console.WriteLine($"ProjectID: {p.ProjectID}, Name: {p.Name}");
            }
            Console.WriteLine("ProjectID 선택(Enter 취소):");
            string idInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(idInput)) return null;

            if (int.TryParse(idInput, out int chosenID))
            {
                var selected = byName.FirstOrDefault(pr => pr.ProjectID == chosenID);
                if (selected != null) return selected;
            }
            Console.WriteLine("유효하지 않은 ProjectID.");
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

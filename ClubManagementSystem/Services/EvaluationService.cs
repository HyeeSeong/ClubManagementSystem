using ClubManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace ClubManagementSystem.Services;

public class EvaluationService
{
    private ClubManagementContext context;

    public EvaluationService(ClubManagementContext context)
    {
        this.context = context;
    }

    public bool InsertEvaluation()
    {
        try
        {
            // Professor 선택
            var professorID = GetUserIDByRole("Professor");
            if (professorID == null) return true;

            // Project 선택
            var projectID = GetProjectID();
            if (projectID == null) return true;

            decimal score = GetInput<decimal>("Score(소수 포함 가능)", input => decimal.TryParse(input, out decimal v) && v >= 0 && v <= 100, "0~100 범위를 입력하세요.", input => decimal.Parse(input));

            var evaluation = new Evaluation
            {
                ProfessorID = professorID.Value,
                ProjectID = projectID.Value,
                Score = score,
                Date = DateTime.Today
            };

            context.Evaluations.Add(evaluation);
            context.SaveChanges();
            Console.WriteLine("Evaluation 추가 완료.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("오류 발생: " + ex.Message);
            return false;
        }
    }

    public bool UpdateEvaluation()
    {
        try
        {
            var eval = FindEvaluation();
            if (eval == null) return true;

            Console.WriteLine($"Evaluation 선택: EvaluationID = {eval.EvaluationID}, Score = {eval.Score}, Date = {eval.Date}");
            Console.WriteLine("[1. Score 변경, 2. Date 변경, Enter 종료]");
            while (true)
            {
                Console.Write("옵션(Enter 종료): ");
                string option = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(option)) break;

                switch (option)
                {
                    case "1":
                        eval.Score = GetInput<decimal>("새로운 Score(0~100)", input => decimal.TryParse(input, out decimal v) && v >= 0 && v <= 100, "유효하지 않은 Score", input => decimal.Parse(input));
                        Console.WriteLine("Score 변경 완료.");
                        break;
                    case "2":
                        eval.Date = GetInput<DateTime>("새로운 날짜(YYYY-MM-DD)", input => DateTime.TryParse(input, out _), "유효하지 않은 날짜", input => DateTime.Parse(input));
                        Console.WriteLine("날짜 변경 완료.");
                        break;
                    default:
                        Console.WriteLine("유효하지 않은 선택.");
                        break;
                }
            }

            context.SaveChanges();
            Console.WriteLine("Evaluation 업데이트 완료.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("오류 발생: " + ex.Message);
            return false;
        }
    }

    public bool DeleteEvaluation()
    {
        try
        {
            var eval = FindEvaluation();
            if (eval == null) return true;

            Console.WriteLine($"Evaluation 삭제: EvaluationID = {eval.EvaluationID}");
            Console.Write("정말 삭제?(Y/N): ");
            string ans = Console.ReadLine()?.Trim().ToUpper();
            if (ans == "Y")
            {
                context.Evaluations.Remove(eval);
                context.SaveChanges();
                Console.WriteLine("Evaluation 삭제 완료.");
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

    public void PrintAllEvaluations()
    {
        var evals = context.Evaluations.ToList();
        if (!evals.Any())
        {
            Console.WriteLine("등록된 Evaluation이 없습니다.");
            return;
        }

        Console.WriteLine("[Evaluation 목록]");
        Console.WriteLine("======================================");
        foreach (var e in evals)
        {
            Console.WriteLine($"[{e.EvaluationID}] 평가 교수: {e.ProfessorID}, 프로젝트명: {e.Project.Name}, Score: {e.Score}, Date: {e.Date:yyyy-MM-dd}");
        }
        Console.WriteLine("======================================");
    }

    private Evaluation? FindEvaluation()
    {
        while (true)
        {
            Console.WriteLine("Evaluation을 검색합니다. 교수 이름 또는 프로젝트 이름을 입력하세요 (Enter 입력 시 취소):");
            string input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input)) return null;

            // 교수 이름으로 검색
            var professors = context.Professors
                .Include(p => p.User)
                .Where(p => p.User.FirstName.Contains(input) || p.User.LastName.Contains(input))
                .ToList();

            if (professors.Count != 0)
            {
                Console.WriteLine("[검색된 교수 목록]");
                foreach (var prof in professors)
                {
                    Console.WriteLine($"ProfessorID: {prof.UserID}, Name: {prof.User.FirstName} {prof.User.LastName}");
                }

                Console.Write("ProfessorID를 입력하세요 (Enter 입력 시 취소): ");
                string profInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(profInput)) continue;

                if (int.TryParse(profInput, out int professorId))
                {
                    var evaluations = context.Evaluations
                        .Where(e => e.ProfessorID == professorId)
                        .Include(e => e.Project)
                        .ToList();

                    if (!evaluations.Any())
                    {
                        Console.WriteLine("해당 교수와 관련된 Evaluation이 없습니다.");
                        continue;
                    }

                    Console.WriteLine("[교수와 관련된 Evaluation]");
                    foreach (var eval in evaluations)
                    {
                        Console.WriteLine($"[{eval.EvaluationID}] Project: {eval.Project.Name}, Score: {eval.Score}, Date: {eval.Date}");
                    }

                    Console.Write("EvaluationID를 입력하세요 (Enter 입력 시 취소): ");
                    string evalInput = Console.ReadLine()?.Trim();
                    if (string.IsNullOrWhiteSpace(evalInput)) return null;

                    if (int.TryParse(evalInput, out int evaluationId))
                    {
                        var selectedEval = evaluations.FirstOrDefault(e => e.EvaluationID == evaluationId);
                        if (selectedEval != null) return selectedEval;

                        Console.WriteLine("유효하지 않은 EvaluationID입니다.");
                        continue;
                    }
                }
            }

            // 프로젝트 이름으로 검색
            var projects = context.Projects
                .Where(p => p.Name.Contains(input))
                .Include(p => p.Evaluations)
                .ThenInclude(e => e.Professor.User)
                .ToList();

            if (projects.Count != 0)
            {
                Console.WriteLine("[검색된 프로젝트 목록]");
                foreach (var project in projects)
                {
                    Console.WriteLine($"ProjectID: {project.ProjectID}, Name: {project.Name}");
                }

                Console.Write("ProjectID를 입력하세요 (Enter 입력 시 취소): ");
                string projInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(projInput)) continue;

                if (int.TryParse(projInput, out int projectId))
                {
                    var selectedProject = projects.FirstOrDefault(p => p.ProjectID == projectId);
                    if (selectedProject != null)
                    {
                        var evaluations = selectedProject.Evaluations.ToList();

                        if (!evaluations.Any())
                        {
                            Console.WriteLine($"프로젝트 '{selectedProject.Name}'에 관련된 Evaluation이 없습니다.");
                            continue;
                        }

                        Console.WriteLine($"프로젝트 '{selectedProject.Name}'의 Evaluation 목록:");
                        foreach (var eval in evaluations)
                        {
                            Console.WriteLine($"EvaluationID: {eval.EvaluationID}, Professor: {eval.Professor.User.FirstName} {eval.Professor.User.LastName}, Score: {eval.Score}, Date: {eval.Date}");
                        }

                        Console.Write("EvaluationID를 입력하세요 (Enter 입력 시 취소): ");
                        string evalInput = Console.ReadLine()?.Trim();
                        if (string.IsNullOrWhiteSpace(evalInput)) return null;

                        if (int.TryParse(evalInput, out int evaluationId))
                        {
                            var selectedEval = evaluations.FirstOrDefault(e => e.EvaluationID == evaluationId);
                            if (selectedEval != null) return selectedEval;

                            Console.WriteLine("유효하지 않은 EvaluationID입니다.");
                            continue;
                        }
                    }
                }
            }

            Console.WriteLine("교수 이름 또는 프로젝트 이름과 일치하는 결과가 없습니다.");
        }
    }


    private int? GetUserIDByRole(string role) 
    {
        if (role == "Professor")
        {
            var professors = context.Professors.Include(p => p.User).ToList();
            if (!professors.Any())
            {
                Console.WriteLine("등록된 교수가 없습니다.");
                return null;
            }

            Console.WriteLine("[교수 목록]");
            foreach (var prof in professors)
            {
                Console.WriteLine($"[{prof.UserID}] Name: {prof.User.FirstName} {prof.User.LastName}");
            }

            int id = GetInput<int>("Professor UserID", input => professors.Any(pr => pr.UserID == int.Parse(input)), "유효하지 않은 UserID");
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
            Console.WriteLine($"ProjectID: {p.ProjectID}, Name: {p.Name}");
        }

        int pid = GetInput<int>("ProjectID", input => projects.Any(pr => pr.ProjectID == int.Parse(input)), "유효하지 않은 ProjectID");
        return pid;
    }

    private static T GetInput<T>(string prompt, Func<string, bool> validator, string errorMessage = "유효하지 않은 입력입니다.", Func<string,T> parser = null)
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

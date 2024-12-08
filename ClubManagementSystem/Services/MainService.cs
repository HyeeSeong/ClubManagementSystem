using ClubManagementSystem.Domain;

namespace ClubManagementSystem.Services;

public class MainService
{
    public UserService UserService { get; }
    public ClubService ClubService { get; }
    public ClubRoomService ClubRoomService { get; }
    public ProjectService ProjectService { get; }
    public NotificationService NotificationService { get; }
    public ParticipationService ParticipationService { get; }
    public EvaluationService EvaluationService { get; }

    public MainService(ClubManagementContext context)
    {
        UserService = new UserService(context);
        ClubService = new ClubService(context);
        ClubRoomService = new ClubRoomService(context);
        ProjectService = new ProjectService(context);
        NotificationService = new NotificationService(context);
        ParticipationService = new ParticipationService(context);
        EvaluationService = new EvaluationService(context);
    }

    // // 복합 기능 예시: 특정 교수의 모든 Evaluation과 소속 Club 이름들 출력
    // public void PrintProfessorDetails()
    // {
    //     var professorID = EvaluationService.GetUserIDByRole("Professor");
    //     if (professorID == null) return;
    //
    //     // 교수의 Club 리스트
    //     var clubs = UserService.Context.Professors
    //         .Include(p => p.Clubs)
    //         .ThenInclude(c => c.ClubRoom)
    //         .FirstOrDefault(p => p.UserID == professorID)
    //         ?.Clubs;
    //
    //     // 교수의 Evaluation 리스트
    //     var evals = UserService.Context.Evaluations
    //         .Where(e => e.ProfessorID == professorID)
    //         .ToList();
    //
    //     Console.WriteLine("=== 교수 상세 정보 ===");
    //     if (clubs != null && clubs.Any())
    //     {
    //         Console.WriteLine("[담당 Club]");
    //         foreach (var c in clubs)
    //         {
    //             Console.WriteLine($"ClubID: {c.ClubID}, ClubName: {c.ClubName}");
    //         }
    //     }
    //     else
    //     {
    //         Console.WriteLine("담당 Club이 없습니다.");
    //     }
    //
    //     if (evals.Any())
    //     {
    //         Console.WriteLine("[평가 목록]");
    //         foreach (var e in evals)
    //         {
    //             Console.WriteLine($"EvaluationID: {e.EvaluationID}, ProjectID: {e.ProjectID}, Score: {e.Score}, Date: {e.Date}");
    //         }
    //     }
    //     else
    //     {
    //         Console.WriteLine("Evaluation 데이터가 없습니다.");
    //     }
    // }
}

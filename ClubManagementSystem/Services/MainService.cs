using ClubManagementSystem.Domain;

namespace ClubManagementSystem.Services;

public class MainService
{
    public ClubManagementContext context { get; }
    public UserService UserService { get; }
    public ClubService ClubService { get; }
    public ClubRoomService ClubRoomService { get; }
    public ProjectService ProjectService { get; }
    public NotificationService NotificationService { get; }
    public ParticipationService ParticipationService { get; }
    public EvaluationService EvaluationService { get; }

    public MainService(ClubManagementContext context)
    {
        this.context = context; 
        UserService = new UserService(context);
        ClubService = new ClubService(context);
        ClubRoomService = new ClubRoomService(context);
        ProjectService = new ProjectService(context);
        NotificationService = new NotificationService(context);
        ParticipationService = new ParticipationService(context);
        EvaluationService = new EvaluationService(context);
    }
    
    public void Start()
    {
        while (true)
        {
            Console.WriteLine("[메인 메뉴]");
            Console.WriteLine("1. 사용자 관리");
            Console.WriteLine("2. 동아리 관리");
            Console.WriteLine("3. 동아리방 관리");
            Console.WriteLine("4. 프로젝트 관리");
            Console.WriteLine("5. 공지사항 관리");
            Console.WriteLine("6. 고급 기능");
            Console.WriteLine("0. 종료");
            Console.Write("선택: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ManageUsers();
                    break;
                case "2":
                    ManageClubs();
                    break;
                case "3":
                    ManageClubRooms();
                    break;
                case "4":
                    ManageProjects();
                    break;
                case "5":
                    ManageNotifications();
                    break;
                case "6":
                    AdvancedFeatures();
                    break;
                case "0":
                    Console.WriteLine("시스템을 종료합니다.");
                    return;
                default:
                    Console.WriteLine("유효하지 않은 선택입니다.");
                    break;
            }
        }
    }
    private void ManageUsers()
    {
        while (true)
        {
            Console.WriteLine("[사용자 관리]");
            Console.WriteLine("1. 사용자 추가");
            Console.WriteLine("2. 사용자 수정");
            Console.WriteLine("3. 사용자 삭제");
            Console.WriteLine("4. 모든 사용자 출력");
            Console.WriteLine("0. 이전 메뉴");
            Console.Write("선택: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    UserService.InsertUser();
                    break;
                case "2":
                    UserService.UpdateUser();
                    break;
                case "3":
                    UserService.DeleteUser();
                    break;
                case "4":
                    UserService.PrintAllUsers();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("유효하지 않은 선택입니다.");
                    break;
            }
        }
    }
    private void ManageClubs()
    {
        while (true)
        {
            Console.WriteLine("[동아리 관리]");
            Console.WriteLine("1. 동아리 추가");
            Console.WriteLine("2. 동아리 수정");
            Console.WriteLine("3. 동아리 삭제");
            Console.WriteLine("4. 모든 동아리 출력");
            Console.WriteLine("0. 이전 메뉴");
            Console.Write("선택: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ClubService.InsertClub();
                    break;
                case "2":
                    ClubService.UpdateClub();
                    break;
                case "3":
                    ClubService.DeleteClub();
                    break;
                case "4":
                    ClubService.PrintAllClubs();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("유효하지 않은 선택입니다.");
                    break;
            }
        }
    }
    private void ManageClubRooms()
    {
        while (true)
        {
            Console.WriteLine("[동아리방 관리]");
            Console.WriteLine("1. 동아리방 추가");
            Console.WriteLine("2. 동아리방 수정");
            Console.WriteLine("3. 동아리방 삭제");
            Console.WriteLine("4. 모든 동아리방 출력");
            Console.WriteLine("0. 이전 메뉴");
            Console.Write("선택: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ClubRoomService.InsertClubRoom();
                    break;
                case "2":
                    ClubRoomService.UpdateClubRoom();
                    break;
                case "3":
                    ClubRoomService.DeleteClubRoom();
                    break;
                case "4":
                    ClubRoomService.PrintAllClubRooms();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("유효하지 않은 선택입니다.");
                    break;
            }
        }
    }
    private void ManageProjects()
    {
        while (true)
        {
            Console.WriteLine("[프로젝트 관리]");
            Console.WriteLine("1. 프로젝트 추가");
            Console.WriteLine("2. 프로젝트 수정");
            Console.WriteLine("3. 프로젝트 삭제");
            Console.WriteLine("4. 모든 프로젝트 출력");
            Console.WriteLine("5. 리포트 추가");
            Console.WriteLine("6. 리포트 수정");
            Console.WriteLine("7. 리포트 삭제");
            Console.WriteLine("8. 모든 리포트 출력");
            Console.WriteLine("9. 참여 관리");
            Console.WriteLine("10. 평가 관리");
            Console.WriteLine("0. 이전 메뉴");
            Console.Write("선택: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ProjectService.InsertProject();
                    break;
                case "2":
                    ProjectService.UpdateProject();
                    break;
                case "3":
                    ProjectService.DeleteProject();
                    break;
                case "4":
                    ProjectService.PrintAllProjects();
                    break;
                case "5":
                    ProjectService.InsertReport();
                    break;
                case "6":
                    ProjectService.UpdateReport();
                    break;
                case "7":
                    ProjectService.DeleteReport();
                    break;
                case "8":
                    ProjectService.PrintAllReports();
                    break;
                case "9":
                    ManageParticipation();
                    break;
                case "10":
                    ManageEvaluation();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("유효하지 않은 선택입니다.");
                    break;
            }
        }
    }
    private void ManageParticipation()
    {
        while (true)
        {
            Console.WriteLine("[참여 관리]");
            Console.WriteLine("1. 참여 추가");
            Console.WriteLine("2. 참여 삭제");
            Console.WriteLine("3. 모든 참여 출력");
            Console.WriteLine("0. 이전 메뉴");
            Console.Write("선택: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ParticipationService.InsertParticipation();
                    break;
                case "2":
                    ParticipationService.DeleteParticipation();
                    break;
                case "3":
                    ParticipationService.PrintAllParticipations();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("유효하지 않은 선택입니다.");
                    break;
            }
        }
    }
    private void ManageEvaluation()
    {

        while (true)
        {
            Console.WriteLine("[평가 관리]");
            Console.WriteLine("1. 평가 추가");
            Console.WriteLine("2. 평가 수정");
            Console.WriteLine("3. 평가 삭제");
            Console.WriteLine("4. 모든 평가 출력");
            Console.WriteLine("0. 이전 메뉴");
            Console.Write("선택: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    EvaluationService.InsertEvaluation();
                    break;
                case "2":
                    EvaluationService.UpdateEvaluation();
                    break;
                case "3":
                    EvaluationService.DeleteEvaluation();
                    break;
                case "4":
                    EvaluationService.PrintAllEvaluations();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("유효하지 않은 선택입니다.");
                    break;
            }
        }
    }
    private void ManageNotifications()
    {
        while (true)
        {
            Console.WriteLine("[공지사항 관리]");
            Console.WriteLine("1. 공지사항 추가");
            Console.WriteLine("2. 공지사항 수정");
            Console.WriteLine("3. 공지사항 삭제");
            Console.WriteLine("4. 모든 공지사항 출력");
            Console.WriteLine("0. 이전 메뉴");
            Console.Write("선택: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    NotificationService.InsertNotification();
                    break;
                case "2":
                    NotificationService.UpdateNotification();
                    break;
                case "3":
                    NotificationService.DeleteNotification();
                    break;
                case "4":
                    NotificationService.PrintAllNotifications();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("유효하지 않은 선택입니다.");
                    break;
            }
        }
    }
    
    private void AdvancedFeatures()
    {
        while (true)
        {
            Console.WriteLine("[고급 기능]");
            Console.WriteLine("1. 사용자 활동 로그 조회");
            Console.WriteLine("2. 시스템 통계 보기");
            Console.WriteLine("3. 프로젝트 통계 보기");
            Console.WriteLine("0. 이전 메뉴");
            Console.Write("선택: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    UserService.PrintUserActivity();
                    break;
                case "2":
                    PrintSystemStatistics();
                    break;
                case "3":
                    ProjectService.PrintProjectStatistics();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("유효하지 않은 선택입니다.");
                    break;
            }
        }
    }
    
    public void PrintSystemStatistics()
    {
        try
        {
            int totalUsers = context.Users.Count();
            int activeClubs = context.Clubs.Count();
            int activeProjects = context.Projects.Count();
            int totalNotifications = context.Notifications.Count();

            Console.WriteLine("\n[시스템 통계]");
            Console.WriteLine($"- 총 사용자 수: {totalUsers}");
            Console.WriteLine($"- 활성 동아리 수: {activeClubs}");
            Console.WriteLine($"- 진행 중인 프로젝트 수: {activeProjects}");
            Console.WriteLine($"- 총 공지 수: {totalNotifications}");
            Console.WriteLine("====================================");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"오류 발생: {ex.Message}");
        }
    }
}

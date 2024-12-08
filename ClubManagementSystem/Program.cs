using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ClubManagementSystem.Domain;
using ClubManagementSystem.Services;

class Program
{
    static void Main(string[] args)
    {
        // 데이터베이스 컨텍스트 초기화
        var context = new ClubManagementContext();

        // 각 서비스 초기화
        var userService = new UserService(context);
        var clubService = new ClubService(context);
        var clubRoomService = new ClubRoomService(context);
        var projectService = new ProjectService(context);
        var notificationService = new NotificationService(context);
        var participationService = new ParticipationService(context);
        var evaluationService = new EvaluationService(context);

        // 1. UserService 테스트
        Console.WriteLine("=== UserService 테스트 시작 ===");
        userService.InsertUser();
        userService.PrintAllUsers();
        userService.UpdateUser();
        userService.PrintAllUsers();
        userService.DeleteUser();
        userService.PrintAllUsers();
        Console.WriteLine("=== UserService 테스트 종료 ===\n");

        // 2. ClubService 테스트
        Console.WriteLine("=== ClubService 테스트 시작 ===");
        clubService.InsertClub();
        clubService.PrintAllClubs();
        clubService.UpdateClub();
        clubService.PrintAllClubs();
        clubService.DeleteClub();
        clubService.PrintAllClubs();
        Console.WriteLine("=== ClubService 테스트 종료 ===\n");

        // 3. ClubRoomService 테스트
        Console.WriteLine("=== ClubRoomService 테스트 시작 ===");
        clubRoomService.InsertClubRoom();
        clubRoomService.PrintAllClubRooms();
        clubRoomService.UpdateClubRoom();
        clubRoomService.PrintAllClubRooms();
        clubRoomService.DeleteClubRoom();
        clubRoomService.PrintAllClubRooms();
        Console.WriteLine("=== ClubRoomService 테스트 종료 ===\n");

    // 4. ProjectService 테스트
        Console.WriteLine("=== ProjectService 테스트 시작 ===");

        Console.WriteLine("4-1. Project 삽입");
        projectService.InsertProject();
        Console.WriteLine("4-2. 모든 Project 출력");
        projectService.PrintAllProjects();

        Console.WriteLine("4-3. Project 수정");
        projectService.UpdateProject(); // UpdateProject 메서드가 구현되어 있다고 가정
        Console.WriteLine("4-4. 모든 Project 출력");
        projectService.PrintAllProjects();

        Console.WriteLine("4-5. Report 삽입");
        projectService.InsertReport();
        Console.WriteLine("4-6. 모든 Report 출력");
        projectService.PrintAllReports();

        Console.WriteLine("4-7. Report 수정");
        projectService.UpdateReport();
        Console.WriteLine("4-8. 모든 Report 출력");
        projectService.PrintAllReports();
        
        Console.WriteLine("4-9. Report 삭제");
        projectService.DeleteReport();
        Console.WriteLine("4-10. 모든 Report 출력");
        projectService.PrintAllReports();

        Console.WriteLine("4-1. Project 삭제");
        projectService.DeleteProject(); // DeleteProject 메서드가 구현되어 있다고 가정
        Console.WriteLine("4-12. 모든 Project 출력");
        projectService.PrintAllProjects();
        Console.WriteLine("=== ProjectService 테스트 종료 ===\n");

        // 5. NotificationService 테스트
        Console.WriteLine("=== NotificationService 테스트 시작 ===");
        notificationService.InsertNotification();
        notificationService.PrintAllNotifications(); 
        notificationService.UpdateNotification();
        notificationService.PrintAllNotifications();
        notificationService.DeleteNotification();
        notificationService.PrintAllNotifications();
        Console.WriteLine("=== NotificationService 테스트 종료 ===\n");

        // 6. ParticipationService 테스트
        Console.WriteLine("=== ParticipationService 테스트 시작 ===");
        participationService.InsertParticipation();
        participationService.PrintAllParticipations();
        participationService.DeleteParticipation();
        participationService.PrintAllParticipations();
        Console.WriteLine("=== ParticipationService 테스트 종료 ===\n");

        // 7. EvaluationService 테스트
        Console.WriteLine("=== EvaluationService 테스트 시작 ===");
        evaluationService.InsertEvaluation();
        evaluationService.PrintAllEvaluations();
        evaluationService.UpdateEvaluation();
        evaluationService.PrintAllEvaluations();
        evaluationService.DeleteEvaluation();
        evaluationService.PrintAllEvaluations();
        Console.WriteLine("=== EvaluationService 테스트 종료 ===\n");

        Console.WriteLine("모든 테스트 완료.");
    }
}

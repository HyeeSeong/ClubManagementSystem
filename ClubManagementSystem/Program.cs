using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ClubManagementSystem.Domain;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            using var context = new ClubManagementContext();
            
            // 1. DB 연결 확인
            Console.WriteLine("DB 연결 테스트...");
            var userCount = context.Users.Count();
            Console.WriteLine($"User 테이블 레코드 수: {userCount}");
            
            // 2. 특정 User 정보 조회
            var user = context.Users
                .FirstOrDefault(u => u.UserID == 1);
            if (user != null)
            {
                Console.WriteLine($"ID=1인 User: {user.FirstName} {user.LastName}, Email={user.Email}");
            }
            
            
        }
        catch (MySqlConnector.MySqlException ex)
        {
            Console.WriteLine("MySQL 연결에 실패했습니다.");
            Console.WriteLine($"오류 내용: {ex.Message}");
            Environment.Exit(1); // 애플리케이션 종료
        }
        catch (Exception ex)
        {
            Console.WriteLine("예상치 못한 오류가 발생했습니다.");
            Console.WriteLine($"오류 내용: {ex.Message}");
            Environment.Exit(1); // 애플리케이션 종료
        }
    }
}

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ClubManagementSystem.Domain;
using ClubManagementSystem.Services;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            using var context = new ClubManagementContext();
            var service = new UserService(context);
            service.InsertUser();

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

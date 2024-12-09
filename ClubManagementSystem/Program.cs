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
            var context = new ClubManagementContext();
            var mainService = new MainService(context);
            mainService.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine("오류 발생 :" + e);
            throw;
        }
        
    }
}

1
# Club Management System

본 프로젝트는 소프트웨어학부 학부 동아리 관리 시스템을 구축한 프로젝트 입니다.

## 주요 구성 요소

1. **데이터베이스 스키마 (MySQL)**
    - `Users`, `Students`, `Professors`, `Staffs`
    - `Clubs`, `ClubRooms`
    - `Projects`, `Reports`
    - `Notifications`, `Notifies`
    - `Participations`, `Evaluations`

   테이블 생성 및 더미 데이터 삽입을 위한 SQL 스크립트(`setup_club_management.sql`)를 제공합니다.

2. **소스코드**
    - `MainService`를 통해 모든 서비스(예: `UserService`, `ClubService`, `ProjectService`, `NotificationService`, `ParticipationService`, `EvaluationService`)에 접근 가능.
    - ER→Relational로 변환한 스키마를 활용하여, CRUD(Create, Read, Update, Delete), 검색, 통계 기능 수행.
    - 네트워크 기반의 DB 접속을 위해 Entitiy Framework Core 사용

3. **메뉴 기반 인터페이스**
    - 콘솔 기반 메뉴 제공.
    - `MainService.Start()` 실행 시 메인 메뉴 출력:
      ```
      [메인 메뉴]
      1. 사용자 관리
      2. 동아리 관리
      3. 동아리방 관리
      4. 프로젝트 관리
      5. 공지사항 관리
      6. 고급 기능
      0. 종료
      ```
    - 각 메뉴 선택 시 하위 메뉴 진입:
        - 사용자 관리: 추가, 수정, 삭제, 조회
        - 동아리 관리: 추가, 수정, 삭제, 조회
        - 프로젝트 관리: 추가, 수정, 삭제, 리포트 관리, 참여/평가 관리
        - 공지사항 관리: 추가, 수정, 삭제, 조회
        - 고급 기능: 사용자 활동 로그, 시스템 통계, 프로젝트 통계

4. **고급 기능**
    - **사용자 활동 로그**: 특정 사용자의 프로젝트 참여 기록, 교수일 경우 평가 내역 출력.
    - **시스템 통계**: 전체 사용자 수, 동아리 수, 진행 중인 프로젝트 수, 공지 수 출력.
    - **프로젝트 통계**: 프로젝트별 참여자 수, 평균 평가 점수 출력.

## 사용 방법

1. **DB 설정**
    - MySQL 서버 준비 (CentOS 등 VM 환경)
    - `setup_club_management.sql` 파일을 사용하여 `club_management` 데이터베이스와 테이블 생성, 더미 데이터 삽입:
      ```bash
      mysql -u [username] -p < setup_club_management.sql
      ```

2. **소스코드 빌드 및 실행(visual studio 기준)**
   1. 해당 프로젝트의 폴더를 다운로드하거나 클론
   2. ClubManagementSystem.sln 를 실행
   4. NuGet 패키지 설치
      필요한 패키지 추가:
      Visual Studio의 NuGet 패키지 관리자를 사용하여 EF Core 및 MySQL 관련 패키지를 설치합니다.
   도구 > NuGet 패키지 관리자 > 패키지 관리자 콘솔을 선택.
   다음 명령어로 패키지를 설치:
   ```
   Install-Package Microsoft.EntityFrameworkCore -Version 8.0.0
   Install-Package Pomelo.EntityFrameworkCore.MySql -Version 8.0.0
   Install-Package MySqlConnector -Version 2.4.0
   ```
   5. Domain 디렉토리의 ClubManagementContext 함수 수정 (자신의 MySQL 주소에 알맞게)
   ```
   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
            optionsBuilder.UseMySql("Server=localhost;" +
                                    "Port=3306;" +
                                    "Database=club_management;" +
                                    "User=hyeseong;" +
                                    "Password=1234;",
                new MySqlServerVersion(new Version(8, 0, 21)));
   }
   ```
   6. 빌드 및 실행

3. **프로그램 실행**
    - 실행하면 메인 메뉴가 콘솔에 표시.
    - 번호를 입력해 원하는 기능 접근.
    - CRUD 작업 및 고급 기능 이용 가능.

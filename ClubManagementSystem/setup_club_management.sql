-- ============================================ 
-- 1. 데이터베이스 생성 및 사용
-- ============================================
DROP DATABASE IF EXISTS club_management;
CREATE DATABASE club_management;
USE club_management;

-- ============================================
-- 2. 테이블 생성
-- ============================================

-- --------------------------------------------
-- 2.1 User 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `User`;
CREATE TABLE Users (
                       UserID INT AUTO_INCREMENT PRIMARY KEY,
                       FirstName VARCHAR(50) NOT NULL,
                       LastName VARCHAR(50) NOT NULL,
                       Email VARCHAR(100) NOT NULL UNIQUE,
                       PhoneNumber VARCHAR(20) DEFAULT '000-0000-0000' NOT NULL,
                       Birth DATE NULL,
                       Description TEXT NULL
);

-- --------------------------------------------
-- 2.2 ClubRoom 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `ClubRoom`;
CREATE TABLE ClubRooms (
                           ClubRoomID INT AUTO_INCREMENT PRIMARY KEY,
                           Location VARCHAR(100) NOT NULL,
                           Size INT NOT NULL,
                           Status VARCHAR(20) NOT NULL
);

-- --------------------------------------------
-- 2.3 Professor 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Professor`;
CREATE TABLE Professors (
                            UserID INT AUTO_INCREMENT PRIMARY KEY,
                            FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);

-- --------------------------------------------
-- 2.4 Staff 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Staff`;
CREATE TABLE Staffs (
                        UserID INT AUTO_INCREMENT PRIMARY KEY,
                        FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);

-- --------------------------------------------
-- 2.5 Club 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Club`;
CREATE TABLE Clubs (
                       ClubID INT AUTO_INCREMENT PRIMARY KEY,
                       ClubName VARCHAR(100) NOT NULL,
                       ClubRoomID INT NULL,
                       ProfessorID INT NOT NULL,
                       StaffID INT NOT NULL,
                       FOREIGN KEY (ClubRoomID) REFERENCES ClubRooms(ClubRoomID) ON DELETE SET NULL,
                       FOREIGN KEY (ProfessorID) REFERENCES Professors(UserID) ON DELETE RESTRICT,
                       FOREIGN KEY (StaffID) REFERENCES Staffs(UserID) ON DELETE RESTRICT
);

-- --------------------------------------------
-- 2.6 Student 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Student`;
CREATE TABLE Students (
                          UserID INT AUTO_INCREMENT PRIMARY KEY,
                          Status VARCHAR(20) NOT NULL,
                          Year INT NOT NULL DEFAULT 1,
                          ClubID INT NULL,
                          FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
                          FOREIGN KEY (ClubID) REFERENCES Clubs(ClubID) ON DELETE SET NULL
);

-- --------------------------------------------
-- 2.7 Project 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Project`;
CREATE TABLE Projects (
                          ProjectID INT AUTO_INCREMENT PRIMARY KEY,
                          Name VARCHAR(100) NOT NULL,
                          ClubID INT NOT NULL,
                          FOREIGN KEY (ClubID) REFERENCES Clubs(ClubID) ON DELETE CASCADE
);

-- --------------------------------------------
-- 2.8 Notification 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Notification`;
CREATE TABLE Notifications (
                               AnnounceID INT AUTO_INCREMENT PRIMARY KEY,
                               Description TEXT NOT NULL,
                               Date DATE NOT NULL DEFAULT (CURRENT_DATE)
);

-- --------------------------------------------
-- 2.9 Report 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Report`;
CREATE TABLE Reports (
                         ReportID INT AUTO_INCREMENT PRIMARY KEY,
                         ProjectID INT NOT NULL,
                         Date DATE NOT NULL,
                         Description TEXT NULL,
                         FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID) ON DELETE CASCADE
);

-- --------------------------------------------
-- 2.10 Participation 테이블 (N:M 관계)
-- --------------------------------------------
DROP TABLE IF EXISTS `Participation`;
CREATE TABLE Participations (
                                StudentID INT NOT NULL,
                                ProjectID INT NOT NULL,
                                PRIMARY KEY (StudentID, ProjectID),
                                FOREIGN KEY (StudentID) REFERENCES Students(UserID) ON DELETE CASCADE,
                                FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID) ON DELETE CASCADE
);

-- --------------------------------------------
-- 2.11 Evaluation 테이블 (N:M 관계)
-- --------------------------------------------
DROP TABLE IF EXISTS `Evaluation`;
CREATE TABLE Evaluations (
                             EvaluationID INT PRIMARY KEY AUTO_INCREMENT,
                             ProfessorID INT NOT NULL,
                             ProjectID INT NOT NULL,
                             Score DECIMAL(5,2) NOT NULL,
                             Date DATE NOT NULL DEFAULT (CURRENT_DATE),
                             FOREIGN KEY (ProfessorID) REFERENCES Professors(UserID) ON DELETE CASCADE,
                             FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID) ON DELETE CASCADE
);

-- --------------------------------------------
-- 2.12 Notifies 테이블 (N:M 관계)
-- --------------------------------------------
DROP TABLE IF EXISTS `Notifies`;
CREATE TABLE Notifies (
                          NotificationID INT NOT NULL,
                          UserID INT NOT NULL,
                          PRIMARY KEY (NotificationID, UserID),
                          FOREIGN KEY (NotificationID) REFERENCES Notifications(AnnounceID) ON DELETE CASCADE,
                          FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);

-- ============================================
-- 3. 더미 데이터 삽입
-- ============================================

-- Users 테이블
INSERT INTO Users (FirstName, LastName, Email, PhoneNumber, Birth, Description)
VALUES
    ('에도가와', '코난', 'conan@example.com', '010-1111-1111', '2010-06-01', '초등학생 탐정입니다.'),
    ('남', '도일', 'kudo@example.com', '010-2222-2222', '1994-05-04', '고등학생이자 명탐정입니다.'),
    ('유', '미란', 'ran@example.com', '010-3333-3333', '1994-10-02', '신이치의 오랜 친구이며, 가라테 선수입니다.'),
    ('신', '짱구', 'shinnosuke@example.com', '010-4444-4444', '1997-05-05', '활발한 유치원생입니다.'),
    ('한', '유리', 'yuri@example.com', '010-5555-5555', '1995-01-15', '짱구의 여동생입니다.'),
    ('신', '형만', 'hiroshi@example.com', '010-6666-6666', '1970-08-01', '짱구의 아버지입니다.'),
    ('봉', '미선', 'misae@example.com', '010-7777-7777', '1975-02-03', '짱구의 어머니입니다.'),
    ('홍', '장미', 'ai@example.com', '010-8888-8888', '2000-01-01', '코난의 동료이며, 약물 연구를 도왔습니다.'),
    ('브라운', '박사', 'agasa@example.com', '010-9999-9999', '1950-05-05', '코난의 조력자입니다.'),
    ('한', '수지', 'suzi@example.com', '010-0000-0000', '1997-05-05', '짱구의 여자친구 입니다');

-- Students 테이블
INSERT INTO Students (UserID, Status, Year, ClubID)
VALUES
    (1, '활동 중', 1, NULL),
    (4, '활동 중', 1, NULL),
    (8, '활동 중', 2, NULL);

-- Professors 테이블
INSERT INTO Professors (UserID)
VALUES
    (2), -- 신이치
    (9); -- 아가사 박사

-- Staff 테이블
INSERT INTO Staffs (UserID)
VALUES
    (3), -- 란
    (6); -- 히로시

-- ClubRooms 테이블
INSERT INTO ClubRooms (Location, Size, Status)
VALUES
    ('A동 101호', 30, '사용 가능'),
    ('B동 202호', 50, '점유 중'),
    ('C동 303호', 20, '청소 중');

-- Clubs 테이블
INSERT INTO Clubs (ClubName, ClubRoomID, ProfessorID, StaffID)
VALUES
    ('탐정 동아리', 1, 2, 3),
    ('어린이 과학 모임', 2, 9, 6);

-- Projects 테이블
INSERT INTO Projects (Name, ClubID)
VALUES
    ('첫 번째 사건 해결 프로젝트', 1),
    ('발명품 개선 프로젝트', 2);

-- Notifications 테이블
INSERT INTO Notifications (Description, Date)
VALUES
    ('탐정 동아리에 오신 것을 환영합니다!', (CURRENT_DATE)),
    ('어린이 과학 모임이 다음 주에 열립니다.', (CURRENT_DATE));

-- Reports 테이블
INSERT INTO Reports (ProjectID, Date, Description)
VALUES
    (1, CURRENT_DATE(), '첫 번째 사건: 보석 살인 사건을 해결했습니다.'),
    (2, CURRENT_DATE(), '발명품의 성능을 개선한 결과를 발표했습니다.');

-- Participations 테이블
INSERT INTO Participations (StudentID, ProjectID)
VALUES
    (1, 1),
    (4, 1),
    (8, 2);

-- Evaluations 테이블
INSERT INTO Evaluations (ProfessorID, ProjectID, Score, Date)
VALUES
    (2, 1, 95.5, (CURRENT_DATE)),
    (9, 2, 88.0, (CURRENT_DATE));

-- Notifies 테이블
INSERT INTO Notifies (NotificationID, UserID)
VALUES
    (1, 1),
    (1, 4),
    (2, 8);

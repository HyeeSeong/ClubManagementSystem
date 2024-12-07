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
CREATE TABLE `User` (
                        UserID INT PRIMARY KEY AUTO_INCREMENT,
                        FirstName VARCHAR(50) NOT NULL,
                        LastName VARCHAR(50) NOT NULL,
                        Email VARCHAR(100) NOT NULL UNIQUE,
                        PhoneNumber VARCHAR(20) NOT NULL DEFAULT '000-0000-0000',
                        Birth DATE NULL,
                        Description TEXT NULL
);

-- --------------------------------------------
-- 2.2 ClubRoom 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `ClubRoom`;
CREATE TABLE `ClubRoom` (
                            ClubRoomID INT PRIMARY KEY AUTO_INCREMENT,
                            Location VARCHAR(100) NOT NULL,
                            Size INT NOT NULL,
                            Status VARCHAR(20) NOT NULL
);

-- --------------------------------------------
-- 2.3 Professor 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Professor`;
CREATE TABLE `Professor` (
                             UserID INT PRIMARY KEY,
                             FOREIGN KEY (UserID) REFERENCES `User`(UserID) ON DELETE CASCADE
);

-- --------------------------------------------
-- 2.4 Staff 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Staff`;
CREATE TABLE `Staff` (
                         UserID INT PRIMARY KEY,
                         FOREIGN KEY (UserID) REFERENCES `User`(UserID) ON DELETE CASCADE
);

-- --------------------------------------------
-- 2.5 Club 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Club`;
CREATE TABLE `Club` (
                        ClubID INT PRIMARY KEY AUTO_INCREMENT,
                        ClubName VARCHAR(100) NOT NULL,
                        ClubRoomID INT NULL,
                        ProfessorID INT NOT NULL,
                        StaffID INT NOT NULL,
                        FOREIGN KEY (ClubRoomID) REFERENCES `ClubRoom`(ClubRoomID) ON DELETE SET NULL,
                        FOREIGN KEY (ProfessorID) REFERENCES `Professor`(UserID) ON DELETE RESTRICT,
                        FOREIGN KEY (StaffID) REFERENCES `Staff`(UserID) ON DELETE RESTRICT
);

-- --------------------------------------------
-- 2.6 Student 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Student`;
CREATE TABLE `Student` (
                           UserID INT PRIMARY KEY,
                           Status VARCHAR(20) NOT NULL,
                           Year INT NOT NULL DEFAULT 1,
                           ClubID INT NULL,
                           FOREIGN KEY (UserID) REFERENCES `User`(UserID) ON DELETE CASCADE,
                           FOREIGN KEY (ClubID) REFERENCES `Club`(ClubID) ON DELETE SET NULL
);

-- --------------------------------------------
-- 2.7 Project 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Project`;
CREATE TABLE `Project` (
                           ProjectID INT PRIMARY KEY AUTO_INCREMENT,
                           Name VARCHAR(100) NOT NULL,
                           ClubID INT NOT NULL,
                           FOREIGN KEY (ClubID) REFERENCES `Club`(ClubID) ON DELETE CASCADE
);

-- --------------------------------------------
-- 2.8 Notification 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Notification`;
CREATE TABLE `Notification` (
                                AnnounceID INT PRIMARY KEY AUTO_INCREMENT,
                                Description TEXT NOT NULL,
                                Date DATE NOT NULL DEFAULT (CURRENT_DATE)
);

-- --------------------------------------------
-- 2.9 Report 테이블
-- --------------------------------------------
DROP TABLE IF EXISTS `Report`;
CREATE TABLE `Report` (
                          ReportID INT PRIMARY KEY AUTO_INCREMENT,
                          ProjectID INT NOT NULL,
                          Date DATE NOT NULL,
                          Description TEXT NULL,
                          FOREIGN KEY (ProjectID) REFERENCES `Project`(ProjectID) ON DELETE CASCADE
);

-- --------------------------------------------
-- 2.10 Participation 테이블 (N:M 관계)
-- --------------------------------------------
DROP TABLE IF EXISTS `Participation`;
CREATE TABLE `Participation` (
                                 StudentID INT,
                                 ProjectID INT,
                                 PRIMARY KEY (StudentID, ProjectID),
                                 FOREIGN KEY (StudentID) REFERENCES `Student`(UserID) ON DELETE CASCADE,
                                 FOREIGN KEY (ProjectID) REFERENCES `Project`(ProjectID) ON DELETE CASCADE
);

-- --------------------------------------------
-- 2.11 Evaluation 테이블 (N:M 관계)
-- --------------------------------------------
DROP TABLE IF EXISTS `Evaluation`;
CREATE TABLE `Evaluation` (
                              EvaluationID INT AUTO_INCREMENT PRIMARY KEY,
                              ProfessorID INT NOT NULL,
                              ProjectID INT NOT NULL,
                              Score DECIMAL(5,2) NOT NULL,
                              Date DATE NOT NULL DEFAULT (CURRENT_DATE),
                              FOREIGN KEY (ProfessorID) REFERENCES `Professor`(UserID) ON DELETE CASCADE,
                              FOREIGN KEY (ProjectID) REFERENCES `Project`(ProjectID) ON DELETE CASCADE
);

-- --------------------------------------------
-- 2.12 Notifies 테이블 (N:M 관계)
-- --------------------------------------------
DROP TABLE IF EXISTS `Notifies`;
CREATE TABLE `Notifies` (
                            NotificationID INT,
                            UserID INT,
                            PRIMARY KEY (NotificationID, UserID),
                            FOREIGN KEY (NotificationID) REFERENCES `Notification`(AnnounceID) ON DELETE CASCADE,
                            FOREIGN KEY (UserID) REFERENCES `User`(UserID) ON DELETE CASCADE
);

-- ============================================
-- 3. 더미 데이터 삽입
-- ============================================

-- --------------------------------------------
-- 3.1 User 테이블 더미 데이터
-- --------------------------------------------
INSERT INTO `User` (UserID, FirstName, LastName, Email, PhoneNumber, Birth, Description) VALUES
                                                                                             (1, '신이치', '코고로', 'shinichi@example.com', '123-4567-8901', '2004-08-08', '고등학생 탐정, 소프트웨어학과 학생.'),
                                                                                             (2, '란', '마도카', 'ran@example.com', '123-5678-9012', '2004-05-04', '범죄심리학과 학생, 신이치의 연인.'),
                                                                                             (3, '모리', '레이', 'mori@example.com', '345-6789-0123', '1970-10-24', '소프트웨어학과 교수, 신이치의 아버지.'),
                                                                                             (4, '아사카', '츠바사', 'tsubasa@example.com', '456-7890-1234', '1996-04-18', '소프트웨어학과 대학원생, 연구 보조.'),
                                                                                             (5, '하이바라', '레이', 'haibara@example.com', '567-8901-2345', '1985-07-04', '소프트웨어학과 5학년 재학중, 전직 블랙 오거나이제이션.'),
                                                                                             (6, '아이즈', '미사오', 'misao@example.com', '123-4567-8901', '1997-11-24', '소프트웨어학과 편입생, 탐정 활동.'),
                                                                                             (7, '쿠로바', '아키라', 'akira@example.com', '123-5678-9012', '1998-10-02', '범죄심리학과 학생, 명탐정 코난의 친구.'),
                                                                                             (8, '아라미', '유명한', 'arami@example.com', '123-5678-9012', '1995-08-09', '소프트웨어학과 명예 교수, 코난의 멘토.');

-- --------------------------------------------
-- 3.2 Professor 테이블 더미 데이터
-- --------------------------------------------
INSERT INTO `Professor` (UserID) VALUES
                                     (3),
                                     (8);

-- --------------------------------------------
-- 3.3 Staff 테이블 더미 데이터
-- --------------------------------------------
INSERT INTO `Staff` (UserID) VALUES
    (4);

-- --------------------------------------------
-- 3.4 ClubRoom 테이블 더미 데이터
-- --------------------------------------------
INSERT INTO `ClubRoom` (ClubRoomID, Location, Size, Status) VALUES
                                                                (101, '건물 A, 201호', 30, '사용 가능'),
                                                                (102, '건물 B, 305호', 25, '점유 중');

-- --------------------------------------------
-- 3.5 Club 테이블 더미 데이터
-- --------------------------------------------
INSERT INTO `Club` (ClubID, ClubName, ClubRoomID, ProfessorID, StaffID) VALUES
                                                                            (1001, '로보틱스 클럽', 101, 3, 4),
                                                                            (1002, '수학 동아리', 102, 3, 4);

-- --------------------------------------------
-- 3.6 Student 테이블 더미 데이터
-- --------------------------------------------
INSERT INTO `Student` (UserID, Status, Year, ClubID) VALUES
                                                         (1, '활동 중', 3, 1001),
                                                         (2, '활동 중', 2, 1002),
                                                         (5, '활동 중', 1, NULL), -- 학생 5는 현재 클럽에 소속되지 않음
                                                         (6, '활동 중', 1, 1001),
                                                         (7, '활동 중', 2, 1002);

-- --------------------------------------------
-- 3.7 Project 테이블 더미 데이터
-- --------------------------------------------
INSERT INTO `Project` (ProjectID, Name, ClubID) VALUES
                                                    (5001, '자율 드론', 1001),
                                                    (5002, '수학적 모델링', 1002),
                                                    (5003, '수사 분석 시스템', 1001);

-- --------------------------------------------
-- 3.8 Notification 테이블 더미 데이터
-- --------------------------------------------
INSERT INTO `Notification` (AnnounceID, Description, Date) VALUES
                                                               (9001, '금요일 오후 5시에 회의가 있습니다.', '2024-11-20'),
                                                               (9002, '프로젝트 제출 기한이 연장되었습니다.', '2024-11-25'),
                                                               (9003, '새로운 프로젝트 아이디어 공모전이 시작되었습니다.', '2024-11-30');

-- --------------------------------------------
-- 3.9 Report 테이블 더미 데이터
-- --------------------------------------------
INSERT INTO `Report` (ReportID, ProjectID, Date, Description) VALUES
                                                                  (7001, 5001, '2024-11-15', '자율 드론 프로젝트의 진행 상황 보고.'),
                                                                  (7002, 5002, '2024-11-18', '수학적 모델링 초기 결과.'),
                                                                  (7003, 5003, '2024-11-20', '수사 분석 시스템 개발 진행 중.');

-- --------------------------------------------
-- 3.10 Participation 테이블 더미 데이터
-- --------------------------------------------
INSERT INTO `Participation` (StudentID, ProjectID) VALUES
                                                       (1, 5001),
                                                       (2, 5002),
                                                       (5, 5001), -- 학생 5도 자율 드론 프로젝트에 참여
                                                       (6, 5003),
                                                       (7, 5002);

-- --------------------------------------------
-- 3.11 Evaluation 테이블 더미 데이터
-- --------------------------------------------
INSERT INTO `Evaluation` (ProfessorID, ProjectID, Score, Date) VALUES
                                                                   (3, 5001, 85.50, '2024-11-20'),
                                                                   (3, 5002, 90.00, '2024-11-22'),
                                                                   (8, 5003, 88.75, '2024-11-25');

-- --------------------------------------------
-- 3.12 Notifies 테이블 더미 데이터
-- --------------------------------------------
INSERT INTO `Notifies` (NotificationID, UserID) VALUES
                                                    (9001, 1),
                                                    (9001, 2),
                                                    (9001, 3),
                                                    (9002, 1),
                                                    (9002, 3),
                                                    (9002, 4),
                                                    (9002, 5),
                                                    (9003, 1),
                                                    (9003, 2),
                                                    (9003, 3),
                                                    (9003, 4),
                                                    (9003, 5),
                                                    (9003, 6),
                                                    (9003, 7),
                                                    (9003, 8);

-- ============================================
-- 4. AUTO_INCREMENT 값 조정
-- ============================================

ALTER TABLE `User` AUTO_INCREMENT = 9;
ALTER TABLE `ClubRoom` AUTO_INCREMENT = 103;
ALTER TABLE `Club` AUTO_INCREMENT = 1003;
ALTER TABLE `Project` AUTO_INCREMENT = 5004;
ALTER TABLE `Notification` AUTO_INCREMENT = 9004;
ALTER TABLE `Report` AUTO_INCREMENT = 7004;
ALTER TABLE `Evaluation` AUTO_INCREMENT = 4;

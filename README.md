CMCS: Contractor Monthly Claim System
1. Project Title
CMCS: Contractor Monthly Claim System (ASP.NET Core 8 MVC)

2. Project Description
CMCS is a web application built using ASP.NET Core 8 MVC designed to streamline and secure the independent contractor claim and approval process within an academic institution. It replaces manual, paper based workflows with a digitized, multi stage approval system.
The application adheres strictly to the constraints of the academic brief:
No Database: All application data (claims, lecturer information) is persisted using encrypted JSON files for secure storage on the file system.
No Roles/Identity: User access is simulated through direct navigation
Core Feature: Implements a full two-stage approval workflow (Programme Co-ordinator $\rightarrow$ Academic Manager) and full CRUD management for Lecturer records.

3. Getting Started
Follow these steps to set up and run the CMCS application in your local environment.
Prerequisites
Visual Studio 2022 (Version 17.14.36518.9 or later).
.NET 8.0 SDK.
Installation Steps
Clone the Repository:
Bash
git clone <https://github.com/WandileSimamane/Contract-Monthly-Claim-System-Part-2>
Open the Solution:
Navigate to the cloned folder and open the CMCS.sln file in Visual Studio.
Build and Run:
Ensure the active debug profile is set to https.
Press F5 or click the 'Start' button to build and launch the application.

4. Project Structure
The codebase follows the standard ASP.NET Core MVC pattern with clear separation of concerns, plus dedicated folders for Data, Models, and Services:
Folder/FileDescriptionControllers/Contains all MVC controllers, including Home, ClaimStatus (Lecturer), ProgrammeCoOrdinator, AcademicManager, and LecturerInformation (HR CRUD).Models/Contains all C# data models, including ClaimModel.cs, LecturerModel.cs, and ClaimInputViewModel.cs.Services/Contains core logic abstractions, notably FileUploadService.cs for file validation and saving.Data/Contains the DataRepository.cs, which acts as the application's secure, file-based "database."Security/Contains SecurityHelper.cs for handling AES encryption/decryption of stored data.Views/Contains Razor views (.cshtml) implementing the Bootstrap-based UI for all roles.appsettings.jsonApplication configuration and logging settings.Program.csApplication entry point and service configuration (registers DataRepository as Singleton).

5. Configuration
Data Storage
Data is saved to the application root using an encrypted JSON file named claims_data_secure.json. This file is generated automatically upon the first successful data save.
Logging
Default logging is configured to output Information level events to the console, with Microsoft.AspNetCore logging set to Warning.

6. Usage & Workflow
The system supports two primary user groups, accessible via the main dashboard links:
A. Lecturer Claim Submission
Submit Claim: Navigate to Submit New Claim.
Input: Enter Total Hours Worked and Hourly Rate.
Upload: Attach a mandatory Supporting Document (restricted to .pdf, .docx, .xlsx, max 5MB).
Tracking: Claims automatically move to the Track Approvals dashboard, showing progress (e.g., Awaiting Co-ordinator Verification).
B. Approval Workflow (PC $\rightarrow$ AM)
Programme Co-ordinator (PC) Review: The PC views claims with status AwaitingPCVerification. The PC can Verify (moves to AM) or Reject (terminates workflow).
Academic Manager (AM) Approval: The AM views claims with status AwaitingAMApproval. The AM performs the Final Approval or Final Rejection.
Document Access: Both the PC and AM roles have the ability to View/Download the securely stored supporting documents for verification.

7. Dependencies
The project relies on the following key NuGet packages:
Microsoft.AspNetCore.Mvc.Core: Core MVC framework.
System.Text.Json: Used for serializing and deserializing application data to and from JSON files.
System.Security.Cryptography: Used in SecurityHelper.cs to implement AES Encryption for secure data storage.

8. Testing
The application includes a dedicated test project (CMCSTest) using MSTest and Moq for mocking dependencies.
Open the Test Explorer in Visual Studio (Test -> Test Explorer).
Run Tests: Select Run All Tests to verify the core logic for the AcademicManagerController, LecturerInformationController, and ClaimStatusController.

9. Acknowledgements
C# AES Encryption Helper Class logic derived from Gemini. Chat Link: <https://gemini.google.com/share/7aca2da078b3>
Code Commenting done by Gemini. Chat Link: <https://gemini.google.com/share/5f00b55ca3bb>
Academic Concepts: Core concepts and architecture were taken from the class repository providing guidance on project structure. Repo link: <https://github.com/fb-shaik/PROG6221-Group2-2025/tree/main>

10. Demo Video
YouTube Demo Link: https://www.youtube.com/watch?v=c-Gk53x16eQ
11. Screenshots / Demo
CMCS Application Home Dashboard:
 <img width="1918" height="1032" alt="Screenshot 2025-10-22 204704" src="https://github.com/user-attachments/assets/bfa4c101-459a-4401-9a5d-3daa0847c61e" />


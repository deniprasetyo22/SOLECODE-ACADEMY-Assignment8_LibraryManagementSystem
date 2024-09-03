-- Table: Workflow
CREATE TABLE Workflow (
    WorkflowId SERIAL PRIMARY KEY,
    WorkflowName VARCHAR(255),
    Description TEXT
);

-- Table: Process
CREATE TABLE Process (
    ProcessId SERIAL PRIMARY KEY,
    ProcessName VARCHAR(255),
    Description TEXT,
    StartDate TIMESTAMP,
    EndDate TIMESTAMP
);

-- Table: WorkflowSequences
CREATE TABLE WorkflowSequences (
    StepId SERIAL PRIMARY KEY,
    WorkflowId INT,
    StepOrder INT,
    StepName VARCHAR(255),
    RequiredRole VARCHAR(255),
    FOREIGN KEY (WorkflowId) REFERENCES Workflow(WorkflowId),
	FOREIGN KEY (RequiredRole) REFERENCES public."AspNetRoles"("Id")
);

-- Table: Requests
CREATE TABLE Requests (
    RequestId SERIAL PRIMARY KEY,
    WorkflowId INT,
    RequesterId VARCHAR,
    RequestType VARCHAR(255),
    Status VARCHAR(255),
    CurrentStepId INT,
    RequestDate TIMESTAMP,
    ProcessId INT,
    FOREIGN KEY (WorkflowId) REFERENCES Workflow(WorkflowId),
    FOREIGN KEY (RequesterId) REFERENCES public."AspNetUsers"("Id"),
    FOREIGN KEY (CurrentStepId) REFERENCES WorkflowSequences(StepId),
    FOREIGN KEY (ProcessId) REFERENCES Process(ProcessId)
);

-- Table: WorkflowActions
CREATE TABLE WorkflowActions (
    ActionId SERIAL PRIMARY KEY,
    RequestId INT,
    StepId INT,
    ActorId VARCHAR,
    Action VARCHAR(255),
    ActionDate TIMESTAMP,
    Comments TEXT,
    FOREIGN KEY (RequestId) REFERENCES Requests(RequestId),
    FOREIGN KEY (StepId) REFERENCES WorkflowSequences(StepId),
    FOREIGN KEY (ActorId) REFERENCES public."AspNetUsers"("Id")
);
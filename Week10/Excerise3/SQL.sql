IF OBJECT_ID('dbo.Scores','U') IS NULL
CREATE TABLE dbo.Scores(
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Player      NVARCHAR(64) NULL,
    Score       INT NOT NULL,
    Level       INT NOT NULL,
    DurationSec INT NOT NULL,
    PlayedAt    DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

IF OBJECT_ID('dbo.Settings','U') IS NULL
CREATE TABLE dbo.Settings(
    Id               INT NOT NULL CONSTRAINT PK_Settings PRIMARY KEY CHECK (Id = 1),
    Rows             INT NOT NULL,
    Cols             INT NOT NULL,
    Pairs            INT NOT NULL,
    FlipMs           INT NOT NULL,
    MismatchDelayMs  INT NOT NULL
);

IF NOT EXISTS (SELECT 1 FROM dbo.Settings WHERE Id=1)
INSERT INTO dbo.Settings(Id,Rows,Cols,Pairs,FlipMs,MismatchDelayMs)
VALUES (1,6,6,18,120,600);
GO
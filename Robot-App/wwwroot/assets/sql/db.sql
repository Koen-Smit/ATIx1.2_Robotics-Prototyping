-- Battery table
CREATE TABLE [dbo].[Battery] (
    [ID] INT IDENTITY(1,1) PRIMARY KEY,
    [Percentage] INT NOT NULL
    [Timestamp] DATETIME DEFAULT GETDATE()
);


-- TaskType table
CREATE TABLE [dbo].[TaskType] (
    [ID] INT IDENTITY(1,1) PRIMARY KEY,
    [TypeName] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(255) NULL
);

-- Create Task table with a foreign key reference to TaskType
CREATE TABLE [dbo].[Task] (
    [ID] INT IDENTITY(1,1) PRIMARY KEY,
    [PressedAt] DATETIME DEFAULT GETDATE(),
    [Status] BIT DEFAULT 0,
    [TaskTypeID] INT,
    CONSTRAINT FK_Task_TaskType FOREIGN KEY ([TaskTypeID]) REFERENCES [dbo].[TaskType]([ID])
);

-- voorbeelddata TaskType tabel
INSERT INTO [dbo].[TaskType] ([TypeName], [Description])
VALUES
    ('Medicijnen innemen', 'Taak om medicijnen in te nemen volgens het voorgeschreven schema.'),
    ('Water drinken', 'Taak om regelmatig water te drinken om gehydrateerd te blijven.'),
    ('Eten', 'Taak om dagelijks te eten voor een goede gezondheid.'),
    ('Rust nemen', 'Taak om tijd te nemen voor rust of een dutje.'),
    ('Wandelen', 'Taak om een korte wandeling te maken voor lichamelijke activiteit.'),
    ('Hygiëne', 'Taak om persoonlijke hygiëne te onderhouden, zoals handen wassen.'),
    ('Praten met familie', 'Taak om met familie of vrienden te praten voor sociale interactie.'),
    ('Kleding veranderen', 'Taak om dagelijkse kleding te verwisselen.'),
    ('Naar de dokter gaan', 'Taak om afspraken bij de dokter bij te houden en te bezoeken.'),
    ('Licht aansteken', 'Taak om verlichting aan te zetten in donkere ruimtes voor veiligheid.');


-- voorbeelddata Task tabel
INSERT INTO [dbo].[Task] ([PressedAt], [Status], [TaskTypeID])
VALUES
    (GETDATE(), 0, 1),  -- TaskTypeID 1: "Medicijnen innemen"
    (GETDATE(), 0, 2),  -- TaskTypeID 2: "Water drinken"
    (GETDATE(), 0, 3),  -- TaskTypeID 3: "Eten"
    (GETDATE(), 0, 4),  -- TaskTypeID 4: "Rust nemen"
    (GETDATE(), 0, 5),  -- TaskTypeID 5: "Wandelen"
    (GETDATE(), 0, 6),  -- TaskTypeID 6: "Hygiëne"
    (GETDATE(), 0, 7),  -- TaskTypeID 7: "Praten met familie"
    (GETDATE(), 0, 8),  -- TaskTypeID 8: "Kleding veranderen"
    (GETDATE(), 0, 9),  -- TaskTypeID 9: "Naar de dokter gaan"
    (GETDATE(), 0, 10); -- TaskTypeID 10: "Licht aansteken"



-- JOIN query voorbeeld
SELECT 
t.[ID] AS TaskID,
t.[PressedAt],
t.[Status],
t.[TaskTypeID],
tt.[TypeName] AS TaskTypeName,
tt.[Description] AS TaskTypeDescription
FROM 
[dbo].[TaskList] t
JOIN 
[dbo].[TaskType] tt ON t.[TaskTypeID] = tt.[ID]
ORDER BY 
CAST(t.[PressedAt] AS DATE) DESC, 
CAST(t.[PressedAt] AS TIME) DESC
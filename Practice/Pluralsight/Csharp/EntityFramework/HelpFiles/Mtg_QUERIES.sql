SELECT e.*, c.*
FROM Expansions e
LEFT JOIN Cards c ON c.ExpansionId = e.Id

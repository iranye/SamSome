SELECT e.*, c.*
FROM Expansion e
LEFT JOIN Card c ON c.ExpansionId = e.Id

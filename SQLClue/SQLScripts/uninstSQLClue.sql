IF EXISTS (SELECT * FROM [sys].[asymmetric_keys] 
           WHERE [name] = 'SQLClueAsymmetricKey')
 DROP ASYMMETRIC KEY SQLClueAsymmetricKey

GO


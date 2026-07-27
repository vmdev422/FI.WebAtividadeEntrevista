-- ============================================
-- FI_SP_ConsBeneficiario - CONSULTAR BENEFICIÁRIOS POR CLIENTE
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'FI_SP_ConsBeneficiario')
    DROP PROCEDURE FI_SP_ConsBeneficiario
GO

CREATE PROCEDURE FI_SP_ConsBeneficiario
(
    @IDCLIENTE BIGINT
)
AS
BEGIN
    SELECT 
        ID,
        IDCLIENTE,
        CPF,
        NOME
    FROM BENEFICIARIOS
    WHERE IDCLIENTE = @IDCLIENTE
    ORDER BY ID
END
GO
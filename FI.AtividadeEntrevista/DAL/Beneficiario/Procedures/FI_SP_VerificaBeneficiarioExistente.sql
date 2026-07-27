-- ============================================
-- FI_SP_VerificaBeneficiarioExistente - VERIFICA SE CPF EXISTE EM QUALQUER CLIENTE
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'FI_SP_VerificaBeneficiarioExistente')
    DROP PROCEDURE FI_SP_VerificaBeneficiarioExistente
GO

CREATE PROCEDURE FI_SP_VerificaBeneficiarioExistente
(
    @CPF VARCHAR(14)
)
AS
BEGIN
    SELECT 1
    FROM BENEFICIARIOS WITH(NOLOCK)
    WHERE CPF = @CPF
END
GO



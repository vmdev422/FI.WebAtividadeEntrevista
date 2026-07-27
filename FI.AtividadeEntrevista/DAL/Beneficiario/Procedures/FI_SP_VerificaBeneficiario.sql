-- ============================================
-- FI_SP_VerificaBeneficiario - VERIFICA SE BENEFICIÁRIO EXISTE
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'FI_SP_VerificaBeneficiario')
    DROP PROCEDURE FI_SP_VerificaBeneficiario
GO

CREATE PROCEDURE FI_SP_VerificaBeneficiario
(
    @CPF       VARCHAR(14),
    @IDCLIENTE BIGINT
)
AS
BEGIN
    SELECT 1
    FROM BENEFICIARIOS WITH(NOLOCK)
    WHERE CPF = @CPF AND IDCLIENTE = @IDCLIENTE
END
GO
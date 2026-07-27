-- ============================================
-- FI_SP_VerificaBeneficiarioExistenteComCliente
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'FI_SP_VerificaBeneficiarioExistenteComCliente')
    DROP PROCEDURE FI_SP_VerificaBeneficiarioExistenteComCliente
GO

CREATE PROCEDURE FI_SP_VerificaBeneficiarioExistenteComCliente
(
    @CPF       VARCHAR(14),
    @IDCLIENTE BIGINT
)
AS
BEGIN
    SELECT 1
    FROM BENEFICIARIOS WITH(NOLOCK)
    WHERE CPF = @CPF AND IDCLIENTE <> @IDCLIENTE
END
GO

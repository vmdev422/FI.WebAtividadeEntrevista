-- ============================================
-- FI_SP_VerificaBeneficiarioCliente - VERIFICA SE CPF É DO CLIENTE
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'FI_SP_VerificaBeneficiarioCliente')
    DROP PROCEDURE FI_SP_VerificaBeneficiarioCliente
GO

CREATE PROCEDURE FI_SP_VerificaBeneficiarioCliente
(
    @CPF       VARCHAR(14),
    @IDCLIENTE BIGINT
)
AS
BEGIN
    SELECT 1
    FROM CLIENTES WITH(NOLOCK)
    WHERE ID = @IDCLIENTE AND CPF = @CPF
END
GO

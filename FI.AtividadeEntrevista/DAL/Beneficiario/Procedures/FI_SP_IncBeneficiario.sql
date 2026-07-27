IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'FI_SP_IncBeneficiario')
    DROP PROCEDURE FI_SP_IncBeneficiario
GO

CREATE PROCEDURE FI_SP_IncBeneficiario
(
    @IDCLIENTE BIGINT,
    @CPF       VARCHAR(14),
    @NOME      VARCHAR(100)
)
AS
BEGIN
    -- Verifica se já existe beneficiário com este CPF para este cliente
    IF EXISTS (SELECT 1 FROM BENEFICIARIOS WHERE IDCLIENTE = @IDCLIENTE AND CPF = @CPF)
    BEGIN
        RAISERROR('Beneficiário com este CPF já cadastrado para este cliente.', 16, 1)
        RETURN
    END

    INSERT INTO BENEFICIARIOS (IDCLIENTE, CPF, NOME)
    VALUES (@IDCLIENTE, @CPF, @NOME)

    SELECT SCOPE_IDENTITY() AS ID
END
GO
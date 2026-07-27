IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'FI_SP_AltBeneficiario')
    DROP PROCEDURE FI_SP_AltBeneficiario
GO

CREATE PROCEDURE FI_SP_AltBeneficiario
(
    @ID        BIGINT,
    @IDCLIENTE BIGINT,
    @CPF       VARCHAR(14),
    @NOME      VARCHAR(100)
)
AS
BEGIN
    -- Verifica se já existe outro beneficiário com este CPF para este cliente
    IF EXISTS (SELECT 1 FROM BENEFICIARIOS WHERE IDCLIENTE = @IDCLIENTE AND CPF = @CPF AND ID <> @ID)
    BEGIN
        RAISERROR('Beneficiário com este CPF já cadastrado para este cliente.', 16, 1)
        RETURN
    END

    UPDATE BENEFICIARIOS
    SET 
        CPF = @CPF,
        NOME = @NOME
    WHERE ID = @ID AND IDCLIENTE = @IDCLIENTE
END
GO
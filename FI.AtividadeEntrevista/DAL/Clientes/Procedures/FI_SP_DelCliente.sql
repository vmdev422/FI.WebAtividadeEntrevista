-- ============================================
-- FI_SP_DelCliente - EXCLUI UM CLIENTE E SEUS BENEFICIÁRIOS
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'FI_SP_DelCliente')
    DROP PROCEDURE FI_SP_DelCliente
GO

CREATE PROCEDURE FI_SP_DelCliente
(
    @ID BIGINT
)
AS
BEGIN
    --  PRIMEIRO EXCLUI OS BENEFICIÁRIOS (FK)
    DELETE FROM BENEFICIARIOS WHERE IDCLIENTE = @ID
    
    --  DEPOIS EXCLUI O CLIENTE
    DELETE FROM CLIENTES WHERE ID = @ID
END
GO


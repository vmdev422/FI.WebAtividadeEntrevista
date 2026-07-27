-- ============================================
-- FI_SP_DelBeneficiario - EXCLUIR BENEFICIÁRIO
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'FI_SP_DelBeneficiario')
    DROP PROCEDURE FI_SP_DelBeneficiario
GO

CREATE PROCEDURE FI_SP_DelBeneficiario
(
    @ID BIGINT
)
AS
BEGIN
    DELETE FROM BENEFICIARIOS WHERE ID = @ID
END
GO
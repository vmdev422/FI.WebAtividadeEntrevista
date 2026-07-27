-- Script de criação da coluna CPF na tabela CLIENTES
USE BancoDeDados;
GO

IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'CLIENTES') AND name = 'CPF'
)
BEGIN
    ALTER TABLE CLIENTES
    ADD CPF VARCHAR(14) NOT NULL;
END
GO
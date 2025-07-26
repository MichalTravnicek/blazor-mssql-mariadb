IF NOT EXISTS (
        SELECT *
        FROM sys.databases
        WHERE name = 'Test'
        )

CREATE DATABASE [Test]
USE [Test]

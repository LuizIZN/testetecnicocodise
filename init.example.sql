CREATE USER IF NOT EXISTS exemplo WITH PASSWORD 'exemplo';
GRANT ALL PRIVILEGES ON DATABASE db_exemplo TO exemplo;
SELECT 'Database initialization completed' as status;
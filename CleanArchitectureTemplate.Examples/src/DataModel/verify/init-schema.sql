-- Verify SampleCleanArhitectureTemplate:init-schema on pg

BEGIN;

Select 1/count(*) from information_schema.tables where table_schema = 'public' and table_name = 'users';

ROLLBACK;

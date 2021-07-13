-- Revert SampleCleanArhitectureTemplate:init-schema from pg

BEGIN;

Drop table users CASCADE;

COMMIT;

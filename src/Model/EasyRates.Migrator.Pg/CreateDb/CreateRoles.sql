-- role for executing migration scripts
create role easyrates_migrator login
password 'easyrates_migrator';

-- role for web application
create role easyrates_app login
password 'easyrates_app';

-- it needs for altering default privileges later
-- else we get the error '[42501] ERROR: must be member of role "easyrates_migrator"'
GRANT easyrates_migrator TO postgres;

-- !!! Need connect to service database !!!

revoke all on database easyrates from public;
revoke all on schema public from public;
revoke all on all tables in schema public from public;

-- grant permissions to migrator role

grant connect on database easyrates to easyrates_migrator;
grant all on schema public to easyrates_migrator;
grant all on all tables in schema public to easyrates_migrator;
grant all on all sequences in schema public to easyrates_migrator;
grant all on all functions in schema public to easyrates_migrator;

alter default privileges for role easyrates_migrator in schema public
    grant all on tables to easyrates_migrator;

alter default privileges for role easyrates_migrator in schema public
    grant all on sequences to easyrates_migrator;

alter default privileges for role easyrates_migrator in schema public
    grant all on functions to easyrates_migrator;

-- grant permissions to app role

grant connect on database easyrates to easyrates_app;
grant usage on schema public to easyrates_app;
grant select, insert, update, delete on all tables in schema public to easyrates_app;
grant usage, select on all sequences in schema public to easyrates_app;
grant execute on all functions in schema public to easyrates_app;

alter default privileges for role easyrates_migrator in schema public
    grant select, insert, update, delete on tables to easyrates_app;

alter default privileges for role easyrates_migrator in schema public
    grant usage, select on sequences to easyrates_app;

alter default privileges for role easyrates_migrator in schema public
    grant execute on functions to easyrates_app;

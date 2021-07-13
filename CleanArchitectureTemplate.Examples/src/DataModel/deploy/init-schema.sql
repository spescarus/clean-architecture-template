-- Deploy SampleCleanArhitectureTemplate:init-schema to pg

BEGIN;

CREATE  TABLE public.users ( 
	id                   uuid DEFAULT gen_random_uuid() NOT NULL ,
	username			 varchar  NOT NULL ,
	first_name           varchar  NOT NULL ,
	last_name            varchar  NOT NULL ,
	email       	     varchar  NOT NULL ,
	address1             varchar ,
	address2             varchar ,
	city                 varchar ,
	country              varchar ,
	created_at           timestamp  NOT NULL ,
	updated_at           timestamp  NOT NULL ,
	deleted_at           timestamp  
 );

ALTER TABLE public.users ADD CONSTRAINT  pk_users PRIMARY KEY ( id );

COMMIT;

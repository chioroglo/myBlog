.PHONY: all
all: help

newmig:
	@echo "Adding new migration..."
	./add-migration.sh arg1
miglist:
	@echo "Migration list:"
	./migrations-list.sh

clean:
	dotnet clean

help:
	@echo "Available commands"
	@echo "make newmig MIGRATION_NAME	Adding new database migration"
	@echo "make miglist	Getting list of active migrations on server"
	

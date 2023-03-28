.DEFAULT_GOAL := run

compose:
	docker compose -f ./docker-compose-monitoring.yml up
run:
	dotnet run --configuration Release -p MiniTwit/Server
migrate:
	dotnet ef database update -p ./MiniTwit/Infrastructure/ -s ./MiniTwit/Server/
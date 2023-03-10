FROM python:3

EXPOSE 5000

WORKDIR /usr/src/app

COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt

COPY . .

# Install the DB
RUN apt-get update && apt-get install -y sqlite3 libsqlite3-dev

# Initialize the DB
RUN sqlite3 /tmp/minitwit.db < schema.sql
RUN sqlite3 /tmp/minitwit.db < dump.sql

CMD [ "python", "./minitwit.py" ]

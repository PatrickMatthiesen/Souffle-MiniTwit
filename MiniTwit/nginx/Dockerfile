FROM --platform=amd64 nginx:alpine AS base

RUN apk update && apk upgrade && apk add nano;
COPY ./nginx.conf /etc/nginx/nginx.conf
COPY ./conf.d/souffle.conf /etc/nginx/conf.d/souffle.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon on;"]
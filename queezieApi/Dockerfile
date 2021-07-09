FROM node:lts

# All command are executed from this path
WORKDIR /app

# Copy files
COPY package.json /app
COPY server.js /app

RUN npm install

CMD ["node", "server.js"]

EXPOSE 3000
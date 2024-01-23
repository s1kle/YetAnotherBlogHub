docker-compose \
  -f compose.api.yaml \
  -f compose.db.yaml \
  -f compose.cache.yaml \
  -f compose.identity.yaml \
  -f compose.rabbitmq.yaml \
  -f compose.helpers.yaml \
  up --abort-on-container-exit
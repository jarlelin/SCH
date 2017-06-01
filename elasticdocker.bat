docker run -d -p 9200:9200 -p 9300:9300 -e "http.host=0.0.0.0" -e "transport.host=127.0.0.1" --name my-elastic --net elk docker.elastic.co/elasticsearch/elasticsearch:5.4.0

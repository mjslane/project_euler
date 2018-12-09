# Introduction

This is a project that solves the first challenge from [project euler](https://projecteuler.net/problem=1) for a set of numbers provided by a kafka topic. This requires that the sum of all natural numbers below the provided number that are multiples of three and five be calculated. The answer should then be placed into a kafa topic. 

This project reads messages from a topic containing a series of integers represented as strings in the value field. This is also the key value. As the definition of natural numbers is non-zero, positive numbers anything that isn't a natural number will return 0;

# How to run the code

The code is written in c# and makes use of the .net core runtime docker image. If you do not have docker installed please see the [instructions](https://docs.docker.com/install/) to install it.

In order to run the code an environment file needs to be created called euler.env. This file *must* contain three settings for the jobs topic, the answer topic and the broker respectively.

```bash
KAFKA_JOBS_TOPIC=my_job_topic
KAFKA_ANSWER_TOPIC=my_answer_topic
KAFKA_BROKER=broker:port
```

## SSL
If the broker requires ssl, firstly create an ssl directory and add the ssl key, certificate and ca files to it. Then add the following to the euler.env file.

```bash
CA_FILE_LOCATION=ssl/ca.pem
KEY_FILE_LOCATION=ssl/kafka.key
CERT_FILE_LOCATION=ssl/kafka.cert
```
The docker container can now be ranby using the following command.

`docker-compose run app`

Docker compose build the image and then run the application. As part of building the image docker will run tests in an intermediate, throw away container. If there has been development that means that the tests no longer pass this step will fail and the code should be fixed in order to run the tests.
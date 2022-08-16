pipeline {
    environment {
    imagename = "mylocaldocker3103/suggestionapi"
    dockerImage = ''
  }

    agent any

    stages {
        stage('Cloning Git') {
            steps {
                git 'https://github.com/VadymIan/SuggestionApplication.git'
            }
        }
        stage('Building') {
            steps {
		 sh 'dir'
                 sh 'dotnet build SuggestionApplication.sln --configuration Release'
            }
        }
        stage('Running Tests') {
            steps {
                sh 'dotnet test --logger "trx;LogFileName=TestResult.trx" SuggestionApiTests/SuggestionApiTests.csproj'
            }
            
            post {
                always {
                    ws('/var/lib/jenkins/workspace/SuggestionAPI/SuggestionApiTests/TestResults')
                    {
                        mstest()
                    }                    
                }
            }
        }
	stage('Building Image') {
		steps {
			script {
				dockerImage = docker.build(imagename, "--no-cache .")
			}
		}
	}
	stage('Pushing Image') {
		steps {
			script {
				docker.withRegistry('https://registry.hub.docker.com', 'suggestionapi') {
					dockerImage.push('latest')
				}
			}
		}
	}
    }
}

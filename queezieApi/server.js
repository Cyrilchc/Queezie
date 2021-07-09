const express = require('express');
mysql = require('mysql');
db = mysql.createPool({
    host: '172.20.0.2',
    user: 'root',
    password: 'queezie',
    database: 'queezie'
})

const app = express();
app.use(express.urlencoded({ extended: true }));
app.use(express.json())
app.get('/', (req, res) => {
    res.send(`
    Queezie Api
  `);
})

/**
 * Begin Answers
 */

/**
 * Get answers
 */
app.get('/answers', function (req, res) {
    let sql = `select * from answer`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Get answer by id
 */
app.get('/answers/:id', function (req, res) {
    let sql = `select * from answer where id=${req.params.id}`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Insert answer 
 */
app.post('/answers/', function (req, res) {
    let sql = `insert into answer (answer, type) values('${req.body.answer}', '${req.body.type}');`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.location(`${req.headers.host}/answers/${data.insertId}`)
            res.status(201).json({
                id: data.insertId,
                answer: req.body.answer,
                type: req.body.type
            })
        }
    })
});

/**
 * Delete answer 
 */
app.delete('/answers/:id', function (req, res) {
    let sql = `delete from answer where id='${req.params.id}'`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(204).end()
        }
    })
});

/**
 * End Answers
 */

/**
 * Begin Domains
 */

/**
 * Get domains
 */
app.get('/domains', function (req, res) {
    let sql = `select * from domain`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});


/**
 * Get domain by id
 */
app.get('/domains/:id', function (req, res) {
    let sql = `select * from domain where id=${req.params.id}`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Insert domain 
 */
app.post('/domains/', function (req, res) {
    let sql = `insert into domain (domain) values('${req.body.domain}');`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.location(`${req.headers.host}/domains/${data.insertId}`)
            res.status(201).json({
                id: data.insertId,
                domain: req.body.domain
            })
        }
    })
});

/**
 * Delete domain 
 */
app.delete('/domains/:id', function (req, res) {
    let sql = `delete from domain where id='${req.params.id}'`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(204).end()
        }
    })
});

/**
 * End Domains
 */

/**
 * Begin Link QuestionAnswer
 */

/**
 * Get linked questionanswer
 */
app.get('/linkquestionanswers', function (req, res) {
    let sql = `select * from linkquestionanswer`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Get linked questionanswer by id
 */
app.get('/linkquestionanswers/:id', function (req, res) {
    let sql = `select * from linkquestionanswer where id='${req.params.id}'`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Get linked answer by questionId
 */
app.get('/linkquestionanswers/question/:id', function (req, res) {
    let sql = `select * from linkquestionanswer where questionId='${req.params.id}'`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Get linked answer by answerId
 */
app.get('/linkquestionanswers/answer/:id', function (req, res) {
    let sql = `select * from linkquestionanswer where answerId='${req.params.id}'`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Insert linkQuestionAnswer 
 */
app.post('/linkquestionanswers/', function (req, res) {
    let sql = `insert into linkquestionanswer (questionId, answerId) values ('${req.body.questionId}', '${req.body.answerId}');`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.location(`${req.headers.host}/linkquestionanswers/${data.insertId}`)
            res.status(201).json({
                id: data.insertId,
                questionId: req.body.questionId,
                answerId: req.body.answerId
            })
        }
    })
});

/**
 * Delete linkQuestionAnswer 
 */
app.post('/linkquestionanswers/delete', function (req, res) {
    let sql = `delete from linkquestionanswer where questionId='${req.body.questionId}' and answerId='${req.body.answerId}'`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(204).end()
        }
    })
});

/**
 * End Link QuestionAnswer
 */


/**
 * Begin Link QuizQuestion
 */

/**
 * Get linked QuizQuestion
 */
app.get('/linkquizquestions', function (req, res) {
    let sql = `select * from linkquizquestion`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Get linked QuizQuestion by id
 */
app.get('/linkquizquestions/:id', function (req, res) {
    let sql = `select * from linkquizquestion where id='${req.params.id}'`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Get linked QuizQuestion by quizId
 */
app.get('/linkquizquestions/quiz/:id', function (req, res) {
    let sql = `select * from linkquizquestion where quizId='${req.params.id}'`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Get linked QuizQuestion by answerId
 */
app.get('/linkquizquestions/question/:id', function (req, res) {
    let sql = `select * from linkquizquestion where questionId='${req.params.id}'`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Insert linkQuizQuestion
 */
app.post('/linkquizquestions/', function (req, res) {
    let sql = `insert into linkquizquestion (quizId, questionId) values ('${req.body.quizId}', '${req.body.questionId}');`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.location(`${req.headers.host}/linkquizquestions/${data.insertId}`)
            res.status(201).json({
                id: data.insertId,
                quizId: req.body.quizId,
                questionId: req.body.questionId
            })
        }
    })
});

/**
 * Delete linkQuizQuestion 
 */
app.post('/linkquizquestions/delete', function (req, res) {
    let sql = `delete from linkquizquestion where quizId='${req.body.quizId}' and questionId='${req.body.questionId}'`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(204).end()
        }
    })
});

/**
 * End Link QuizQuestion
 */


/**
 * Begin Questions
 */

/**
 * Get questions
 */
app.get('/questions', function (req, res) {
    let sql = `select * from question`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Get question by id
 */
app.get('/questions/:id', function (req, res) {
    let sql = `select * from question where id=${req.params.id}`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Insert question 
 */
app.post('/questions', function (req, res) {
    let sql = `insert into question (question, questionTypeId, domainId) values('${req.body.question}', '${req.body.questionTypeId}', '${req.body.domainId}');`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.location(`${req.headers.host}/questions/${data.insertId}`)
            res.status(201).json({
                id: data.insertId,
                question: req.body.question,
                questionTypeId: req.body.questionTypeId,
                domainId: req.body.domainId
            })
        }
    })
});

/**
 * Delete question 
 */
app.delete('/questions/:id', function (req, res) {
    let sql = `delete from question where id='${req.params.id}'`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(204).end()
        }
    })
});

/**
 * End Questions
 */

/**
 * Begin QuestionTypes
 */

/**
 * Get QuestionTypes
 */
app.get('/questiontypes', function (req, res) {
    let sql = `select * from questiontype`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Get QuestionTypes by id
 */
app.get('/questiontypes/:id', function (req, res) {
    let sql = `select * from questiontype where id=${req.params.id}`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * End QuestionTypes
 */

/**
 * Begin Quiz
 */

/**
 * Get Quizs
 */
 app.get('/quizs', function (req, res) {
    let sql = `select * from quiz`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Get quiz by id
 */
app.get('/quizs/:id', function (req, res) {
    let sql = `select * from quiz where id=${req.params.id}`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code.code
            })
        }
        else {
            res.status(200).send(data)
        }
    })
});

/**
 * Insert quiz
 */
 app.post('/quizs', function (req, res) {
    let sql = `insert into quiz (quiz, duration) values('${req.body.quiz}', '${req.body.duration}');`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.location(`${req.headers.host}/quizs/${data.insertId}`)
            res.status(201).json({
                id: data.insertId,
                quiz: req.body.quiz,
                duration: req.body.duration
            })
        }
    })
});

/**
 * Delete quiz 
 */
app.delete('/quizs/:id', function (req, res) {
    let sql = `delete from quiz where id='${req.params.id}'`;
    db.query(sql, function (err, data, fields) {
        if (err) {
            res.status(500).json({
                message: err.code
            })
        }
        else {
            res.status(204).end()
        }
    })
});

/**
 * End Quiz
 */

app.listen(3000);
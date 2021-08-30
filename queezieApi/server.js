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
 * Begin Answerss
 */

/**
 * Get answers
 */
app.get('/answers', function (req, res) {
    let sql = "select * from ??";
    let inserts = ['answer']
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    // let sql = "select * from answer where id=${req.params.id}";
    let sql = "select * from ?? where ??=?";
    let inserts = ['answer', 'id', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "insert into ?? (??, ??) values(?, ?);";
    let inserts = ['answer', 'answer', 'type', req.body.answer, req.body.type]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "delete from ?? where ??=?";
    let inserts = ['answer', 'id', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ??";
    let inserts = ['domain']
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ?? where ??=?";
    let inserts = ['domain', 'id', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "insert into ?? (??) values(?);";
    let inserts = ['domain', 'domain', req.body.domain]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "delete from ?? where ??=?";
    let inserts = ['domain', 'id', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ??";
    let inserts = ['linkquestionanswer']
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ?? where ??=?";
    let inserts = ['linkquestionanswer', 'id', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ?? where ??=?";
    let inserts = ['linkquestionanswer', 'questionId', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ?? where ??=?";
    let inserts = ['linkquestionanswer', 'answerId', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "insert into ?? (??, ??) values (?, ?);";
    let inserts = ['linkquestionanswer', 'questionId', 'answerId', req.body.questionId, req.body.answerId]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "delete from ?? where ??=? and ??=?";
    let inserts = ['linkquestionanswer', 'questionId', req.body.questionId, 'answerId', req.body.answerId]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ??";
    let inserts = ['linkquizquestion']
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ?? where ??=?";
    let inserts = ['linkquizquestion', 'id', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ?? where ??=?";
    let inserts = ['linkquizquestion', 'quizId', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ?? where ??=?";
    let inserts = ['linkquizquestion', 'questionId', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "insert into ?? (??, ??) values (?, ?);";
    let inserts = ['linkquizquestion', 'quizId', 'questionId', req.body.quizId, req.body.questionId]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "delete from ?? where ??=? and ??=?";
    let inserts = ['linkquizquestion', 'quizId', req.body.quizId, 'questionId', req.body.questionId]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ??";
    let inserts = ['question']
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ?? where ??=?";
    let inserts = ['question', 'id', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "insert into ?? (??, ??, ??) values(?, ?, ?);";
    let inserts = ['question', 'question', 'questionTypeId', 'domainId', req.body.question, req.body.questionTypeId, req.body.domainId]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "delete from ?? where ??=?";
    let inserts = ['question', 'id', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ??";
    let inserts = ['questiontype']
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ?? where ??=?";
    let inserts = ['questiontype', 'id', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ??";
    let inserts = ['quiz']
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "select * from ?? where ??=?";
    let inserts = ['quiz', 'id', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "insert into ?? (??, ??) values(?, ?);";
    let inserts = ['quiz', 'duration', req.body.quiz, req.body.duration]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
    let sql = "delete from ?? where ??=?";
    let inserts = ['quiz', 'id', req.params.id]
    db.query(mysql.format(sql, inserts), function (err, data, fields) {
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
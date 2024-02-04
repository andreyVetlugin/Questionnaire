CREATE TABLE surveys (
	id uuid  DEFAULT gen_random_uuid() PRIMARY KEY,
  	name varchar(50) UNIQUE NOT NULL,
  	first_question_id uuid UNIQUE,
	is_public bool NOT NULL
);

CREATE TABLE questions (
	id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	survey_id uuid REFERENCES surveys(id) ON DELETE SET NULL,
	next_question_id uuid UNIQUE REFERENCES questions(id),
	question_text text NOT NULL,
	allow_few_answers bool NOT NULL
);

ALTER TABLE surveys 
    ADD CONSTRAINT fk_surveys_questions
    FOREIGN KEY (first_question_id) 
    REFERENCES questions (id)
    ON DELETE SET NULL;

CREATE TABLE interviews (
	id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	survey_id uuid NOT NULL REFERENCES surveys(id) ON DELETE CASCADE,
	next_question_id uuid REFERENCES questions(id)
);

CREATE TABLE answers(
	id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	question_id uuid NOT NULL REFERENCES questions(id) ON DELETE CASCADE,
	answer_text text NOT NULL
);

CREATE TABLE results(
	id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	interview_id uuid REFERENCES interviews(id) ON DELETE CASCADE,
	answer_id uuid REFERENCES answers(id) ON DELETE CASCADE,
	question_id uuid REFERENCES questions(id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX survey_question_idx ON questions (survey_id, question_text);
CREATE UNIQUE INDEX result_unique_idx ON results (interview_id, answer_id, question_id);
CREATE UNIQUE INDEX answer_question_idx ON answers (answer_text, question_id);
CREATE INDEX question_idx ON answers(question_id);
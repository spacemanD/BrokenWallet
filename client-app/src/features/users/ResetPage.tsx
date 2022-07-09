import { ErrorMessage, Form, Formik } from "formik";
import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import { history } from "../..";
import { NavLink } from "react-router-dom";
import { Button, Header, Label } from "semantic-ui-react";
import MyTextInput from "../../app/common/form/MyTextInput";
import { useStore } from "../../app/stores/store";
import * as Yup from 'yup'

export default observer(function ResetPage() {
    const {userStore} = useStore();
    return (
        <Formik
            initialValues={{email: '', error: null}}
            onSubmit={(values, {setErrors}) => 
                userStore.reset(values).catch(error => 
                setErrors({error: 'Email is not existing'}))
                .then(() => history.push('/')) 
            }
            validationSchema={Yup.object({
                email: Yup.string().required().email()
            })}
        >
            {({handleSubmit, isSubmitting, errors, isValid, dirty}) =>(
                <Form className="ui form" onSubmit={handleSubmit} autoComplete='off'>
                    <Header as='h2' content='Enter your email to reset the password' color='teal'  textAlign='center'/>
                    <MyTextInput name='email' placeholder="Email" />
                    <ErrorMessage
                        name='error' render={() => 
                        <Label style={{marginBottom: 10}} basic color='red' content={errors.error}/>}
                    />
                    <Button loading={isSubmitting} 
                    positive content='Submit' 
                    disabled={!isValid || !dirty || isSubmitting}
                    type='submit' fluid />
                    <br/> 
                    <Button onClick={() => window.location.reload()}
                        content='Back' fluid/>
                </Form>
            )}
        </Formik>
    )
})
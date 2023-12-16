'use client'

import { Button, TextInput } from 'flowbite-react';
import React from 'react'
import { FieldValue, FieldValues, useForm } from 'react-hook-form'
import Input from '../components/Input';

export default function AuctionForm() {
  const { register, handleSubmit, setFocus,
    formState: { isSubmitted, isValid, isDirty, errors } } = useForm();

  function onSubmit(data: FieldValues) {
    console.log(data);
  }

  return (
    <form className='flex flex-col mt-3' onSubmit={handleSubmit(onSubmit)}>
      <Input label='Make' name='make' />
      <div className='mb-3 block'>
        <TextInput
          {...register('model', { required: 'Model is required' })}
          placeholder='Model'
          color={errors?.model && 'failure'}
          helperText={errors.model?.message as string}
        />
      </div>
      <div className='flex justify-between'>
        <Button outline color='gray'>Cancel</Button>
        <Button 
          isProcessing={isSubmitted} 
          // disabled={!isValid}
          type='submit'
          outline color='success'>Submit</Button>
      </div>
    </form>
  )
}

# -*- coding: utf-8 -*-

import sys
import unittest

import System

from _compat import PY2, PY3, pickle, text_type


class ExceptionTests(unittest.TestCase):
    """Test exception support."""

    def test_unified_exception_semantics(self):
        """Test unified exception semantics."""
        e = System.Exception('Something bad happened')
        self.assertTrue(isinstance(e, Exception))
        self.assertTrue(isinstance(e, System.Exception))

    def test_standard_exception_attributes(self):
        """Test accessing standard exception attributes."""
        from System import OverflowException
        from Python.Test import ExceptionTest

        e = ExceptionTest.GetExplicitException()
        self.assertTrue(isinstance(e, OverflowException))

        self.assertTrue(e.Message == 'error')

        e.Source = 'Test Suite'
        self.assertTrue(e.Source == 'Test Suite')

        v = e.ToString()
        self.assertTrue(len(v) > 0)

    def test_extended_exception_attributes(self):
        """Test accessing extended exception attributes."""
        from Python.Test import ExceptionTest, ExtendedException
        from System import OverflowException

        e = ExceptionTest.GetExtendedException()
        self.assertTrue(isinstance(e, ExtendedException))
        self.assertTrue(isinstance(e, OverflowException))
        self.assertTrue(isinstance(e, System.Exception))

        self.assertTrue(e.Message == 'error')

        e.Source = 'Test Suite'
        self.assertTrue(e.Source == 'Test Suite')

        v = e.ToString()
        self.assertTrue(len(v) > 0)

        self.assertTrue(e.ExtraProperty == 'extra')
        e.ExtraProperty = 'changed'
        self.assertTrue(e.ExtraProperty == 'changed')

        self.assertTrue(e.GetExtraInfo() == 'changed')

    def test_raise_class_exception(self):
        """Test class exception propagation."""
        from System import NullReferenceException

        def test():
            raise NullReferenceException

        self.assertRaises(NullReferenceException, test)

        try:
            raise NullReferenceException
        except:
            type, value, tb = sys.exc_info()
            self.assertTrue(type is NullReferenceException)
            self.assertTrue(isinstance(value, NullReferenceException))

    def test_raise_class_exception_with_value(self):
        """Test class exception propagation with associated value."""
        from System import NullReferenceException

        def test():
            raise NullReferenceException('Aiiieee!')

        self.assertRaises(NullReferenceException, test)

        try:
            raise NullReferenceException('Aiiieee!')
        except:
            type, value, tb = sys.exc_info()
            self.assertTrue(type is NullReferenceException)
            self.assertTrue(isinstance(value, NullReferenceException))
            self.assertTrue(value.Message == 'Aiiieee!')

    def test_raise_instance_exception(self):
        """Test instance exception propagation."""
        from System import NullReferenceException

        def test():
            raise NullReferenceException()

        self.assertRaises(NullReferenceException, test)

        try:
            raise NullReferenceException()
        except:
            type, value, tb = sys.exc_info()
            self.assertTrue(type is NullReferenceException)
            self.assertTrue(isinstance(value, NullReferenceException))
            self.assertTrue(len(value.Message) > 0)

    def test_raise_instance_exception_with_args(self):
        """Test instance exception propagation with args."""
        from System import NullReferenceException

        def test():
            raise NullReferenceException("Aiieeee!")

        self.assertRaises(NullReferenceException, test)

        try:
            raise NullReferenceException('Aiiieee!')
        except:
            type, value, tb = sys.exc_info()
            self.assertTrue(type is NullReferenceException)
            self.assertTrue(isinstance(value, NullReferenceException))
            self.assertTrue(value.Message == 'Aiiieee!')

    def test_managed_exception_propagation(self):
        """Test propagation of exceptions raised in managed code."""
        from System import Decimal, OverflowException

        def test():
            l = Decimal.ToInt64(Decimal.MaxValue)

        self.assertRaises(OverflowException, test)

    def test_managed_exception_conversion(self):
        """Test conversion of managed exceptions."""
        from System import Exception, OverflowException
        from Python.Test import ExceptionTest

        e = ExceptionTest.GetBaseException()
        self.assertTrue(isinstance(e, Exception))

        e = ExceptionTest.GetExplicitException()
        self.assertTrue(isinstance(e, OverflowException))
        self.assertTrue(isinstance(e, Exception))

        e = ExceptionTest.GetWidenedException()
        self.assertTrue(isinstance(e, OverflowException))
        self.assertTrue(isinstance(e, Exception))

        v = ExceptionTest.SetBaseException(Exception('error'))
        self.assertTrue(v)

        v = ExceptionTest.SetExplicitException(OverflowException('error'))
        self.assertTrue(v)

        v = ExceptionTest.SetWidenedException(OverflowException('error'))
        self.assertTrue(v)

    def test_catch_exception_from_managed_method(self):
        """Test catching an exception from a managed method."""
        from Python.Test import ExceptionTest
        from System import OverflowException

        try:
            ExceptionTest().ThrowException()
        except OverflowException:
            e = sys.exc_info()[1]
            self.assertTrue(isinstance(e, OverflowException))
            return

        raise SystemError('failed to catch exception from managed method')

    def test_catch_exception_from_managed_property(self):
        """Test catching an exception from a managed property."""
        from Python.Test import ExceptionTest
        from System import OverflowException

        try:
            v = ExceptionTest().ThrowProperty
        except OverflowException:
            e = sys.exc_info()[1]
            self.assertTrue(isinstance(e, OverflowException))
            return

        try:
            ExceptionTest().ThrowProperty = 1
        except OverflowException:
            e = sys.exc_info()[1]
            self.assertTrue(isinstance(e, OverflowException))
            return

        raise SystemError('failed to catch exception from managed property')

    def test_catch_exception_managed_class(self):
        """Test catching the managed class of an exception."""
        from System import OverflowException

        try:
            raise OverflowException('overflow')
        except OverflowException:
            return

        raise SystemError('failed to catch managed class exception')

    def test_catch_exception_python_class(self):
        """Test catching the python class of an exception."""
        from System import OverflowException
        if PY3:
            from builtins import Exception
        elif PY2:
            from exceptions import Exception

        try:
            raise OverflowException('overflow')
        except Exception:
            return

        raise SystemError('failed to catch python class exception')

    def test_catch_exception_base_class(self):
        """Test catching the base of an exception."""
        from System import OverflowException, ArithmeticException

        try:
            raise OverflowException('overflow')
        except ArithmeticException:
            return

        raise SystemError('failed to catch base exception')

    def test_catch_exception_nested_base_class(self):
        """Test catching the nested base of an exception."""
        from System import OverflowException, SystemException

        try:
            raise OverflowException('overflow')
        except SystemException:
            return

        raise SystemError('failed to catch nested base exception')

    def test_catch_exception_with_assignment(self):
        """Test catching an exception with assignment."""
        from System import OverflowException

        try:
            raise OverflowException('overflow')
        except OverflowException:
            e = sys.exc_info()[1]
            self.assertTrue(isinstance(e, OverflowException))

    def test_catch_exception_unqualified(self):
        """Test catching an unqualified exception."""
        from System import OverflowException

        try:
            raise OverflowException('overflow')
        except:
            return

        raise SystemError('failed to catch unqualified exception')

    def test_apparent_module_of_exception(self):
        """Test the apparent module of an exception."""
        from System import Exception, OverflowException

        self.assertTrue(Exception.__module__ == 'System')
        self.assertTrue(OverflowException.__module__ == 'System')

    def test_str_of_exception(self):
        """Test the str() representation of an exception."""
        from System import NullReferenceException
        from System import Convert, FormatException
        e = NullReferenceException('')
        self.assertEqual(str(e), '')

        e = NullReferenceException('Something bad happened')
        self.assertTrue(str(e).startswith('Something bad happened'))

        try:
            Convert.ToDateTime('this will fail')
        except FormatException:
            e = sys.exc_info()[1]
            msg = text_type(e).encode("utf8")  # fix for international installation
            self.assertTrue(msg.find(text_type('System.Convert.ToDateTime').encode("utf8")) > -1, msg)

    def test_python_compat_of_managed_exceptions(self):
        """Test if managed exceptions are compatible with Python's implementation
        """
        from System import OverflowException
        msg = "A simple message"

        e = OverflowException(msg)
        self.assertEqual(str(e), msg)
        self.assertEqual(text_type(e), msg)

        self.assertEqual(e.args, (msg,))
        self.assertTrue(isinstance(e.args, tuple))
        if PY3:
            self.assertEqual(repr(e), "OverflowException('A simple message',)")
        elif PY2:
            self.assertEqual(repr(e), "OverflowException(u'A simple message',)")

    def test_exception_is_instance_of_system_object(self):
        """Test behavior of isinstance(<managed exception>, System.Object)."""
        # This is an anti-test, in that this is a caveat of the current
        # implementation. Because exceptions are not allowed to be new-style
        # classes, we wrap managed exceptions in a general-purpose old-style
        # class that delegates to the wrapped object. This makes _almost_
        # everything work as expected, except that an isinstance check against
        # CLR.System.Object will fail for a managed exception (because a new
        # style class cannot appear in the __bases__ of an old-style class
        # without causing a crash in the CPython interpreter). This test is
        # here mainly to remind me to update the caveat in the documentation
        # one day when when exceptions can be new-style classes.

        # This behaviour is now over-shadowed by the implementation of
        # __instancecheck__ (i.e., overloading isinstance), so for all Python
        # version >= 2.6 we expect isinstance(<managed exception>, Object) to
        # be true, even though it does not really subclass Object.
        from System import OverflowException
        from System import Object

        o = OverflowException('error')

        if sys.version_info >= (2, 6):
            self.assertTrue(isinstance(o, Object))
        else:
            self.assertFalse(isinstance(o, Object))

    def test_pickling_exceptions(self):
        exc = System.Exception("test")
        dumped = pickle.dumps(exc)
        loaded = pickle.loads(dumped)

        self.assertEqual(exc.args, loaded.args)

    def test_chained_exceptions(self):
        # TODO: Why is this test PY3 only?
        if PY3:
            from Python.Test import ExceptionTest

            try:
                ExceptionTest.ThrowChainedExceptions()
            except Exception as exc:
                msgs = [
                    "Outer exception",
                    "Inner exception",
                    "Innermost exception",
                ]

                for msg in msgs:
                    self.assertEqual(exc.Message, msg)
                    self.assertEqual(exc.__cause__, exc.InnerException)
                    exc = exc.__cause__

            else:
                self.fail("Test should raise an exception")


def test_suite():
    return unittest.makeSuite(ExceptionTests)

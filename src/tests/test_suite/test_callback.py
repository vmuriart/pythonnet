# -*- coding: utf-8 -*-

import unittest


def simpleDefaultArg(arg='test'):
    return arg


class CallbackTests(unittest.TestCase):
    """Test that callbacks from C# into python work."""

    def test_default_for_null(self):
        """Test that C# can use null for an optional python argument"""
        from Python.Test import CallbackTest

        test_instance = CallbackTest()
        retVal = test_instance.Call_simpleDefaultArg_WithNull(__name__)
        pythonRetVal = simpleDefaultArg(None)
        self.assertEquals(retVal, pythonRetVal)

    def test_default_for_none(self):
        """Test that C# can use no argument for an optional python argument"""
        from Python.Test import CallbackTest

        test_instance = CallbackTest()
        retVal = test_instance.Call_simpleDefaultArg_WithEmptyArgs(__name__)
        pythonRetVal = simpleDefaultArg()
        self.assertEquals(retVal, pythonRetVal)


def test_suite():
    return unittest.makeSuite(CallbackTests)
